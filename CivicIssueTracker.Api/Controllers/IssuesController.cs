using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CivicIssueTracker.Api.Data;
using CivicIssueTracker.Api.DTOs.Issues;
using CivicIssueTracker.Api.Models;
using CivicIssueTracker.Api.Helpers;

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

    // Citizen: Report Issue
    [HttpPost]
    public async Task<IActionResult> Create(CreateIssueRequestDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            ReportedByUserId = userId
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return Ok(issue.Id);
    }

    // Citizen: View My Issues
    [HttpGet("my")]
    public async Task<IActionResult> MyIssues()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var issues = await _context.Issues
            .Where(i => i.ReportedByUserId == userId)
            .Select(i => new IssueResponseDto
            {
                Id = i.Id,
                Title = i.Title,
                Category = i.Category,
                Status = i.Status,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();

        return Ok(issues);
    }

    // Admin / Officer: View All Issues
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _context.Issues
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return Ok(issues);
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
}
