using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CivicIssueTracker.Api.Data;
using CivicIssueTracker.Api.DTOs.Issues;
using CivicIssueTracker.Api.Models;
using CivicIssueTracker.Api.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CivicIssueTracker.Api.Controllers;

[ApiController]
[Route("api/issues")]
[Authorize]
public class IssuesController : ControllerBase
{
    private readonly AppDbContext _context;

    public IssuesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetIssues()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var role =
            User.FindFirstValue(ClaimTypes.Role)
            ?? User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        IQueryable<Issue> query = _context.Issues
            .Include(i => i.AssignedOfficer);

        if (role == "Citizen")
        {
            query = query.Where(i => i.ReportedByUserId == userId);
        }
        else if (role == "Officer")
        {
            query = query.Where(i => i.AssignedOfficerId == userId);
        }
        // Admin → all issues

        var issues = await query.ToListAsync();
        return Ok(issues);
    }

    // Citizen: Report Issue
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateIssueWithImageDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        string? imageUrl = null;

        if (dto.Image != null)
        {
            // Validate file
            var allowedTypes = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Image.FileName).ToLower();

            if (!allowedTypes.Contains(extension))
                return BadRequest("Only JPG and PNG images are allowed");

            if (dto.Image.Length > 5 * 1024 * 1024)
                return BadRequest("Image size must be less than 5MB");

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "issues"
            );
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Image.CopyToAsync(stream);

            imageUrl = $"/uploads/issues/{fileName}";
        }

        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            ImageUrl = imageUrl,
            ReportedByUserId = userId
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return Ok(issue.Id);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Officer")]
    public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] UpdateIssueStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);

        if (issue == null)
            return NotFound("Issue not found");

        if (!IssueStatus.AllowedTransitions.ContainsKey(issue.Status) ||
            !IssueStatus.AllowedTransitions[issue.Status].Contains(dto.NewStatus))
        {
            return BadRequest(
                $"Invalid status transition from {issue.Status} to {dto.NewStatus}"
            );
        }

        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        // Add history record
        var history = new IssueStatusHistory
        {
            IssueId = issue.Id,
            OldStatus = issue.Status,
            NewStatus = dto.NewStatus,
            ChangedByUserId = userId
        };

        issue.Status = dto.NewStatus;

        _context.IssueStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return Ok($"Issue status updated to {dto.NewStatus}");
    }

    [HttpGet("{id}/history")]
    [Authorize]
    public async Task<IActionResult> GetStatusHistory(int id)
    {
        var history = await _context.IssueStatusHistories
            .Where(h => h.IssueId == id)
            .OrderBy(h => h.ChangedAt)
            .Select(h => new IssueStatusHistoryDto
            {
                OldStatus = h.OldStatus,
                NewStatus = h.NewStatus,
                ChangedByUserId = h.ChangedByUserId,
                ChangedAt = h.ChangedAt
            })
            .ToListAsync();

        return Ok(history);
    }

    [HttpPut("{id}/assign/{officerId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignIssue(int id, int officerId)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound("Issue not found");

        if (issue.Status != IssueStatus.Reported)
            return BadRequest("Only reported issues can be assigned");

        var officer = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == officerId && u.Role == "Officer");

        if (officer == null)
            return BadRequest("Invalid officer");

        issue.AssignedOfficerId = officerId;
        issue.Status = IssueStatus.Assigned;

        await _context.SaveChangesAsync();

        return Ok("Issue assigned successfully");
    }

}
