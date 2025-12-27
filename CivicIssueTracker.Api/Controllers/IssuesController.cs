using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CivicIssueTracker.Api.Data;
using CivicIssueTracker.Api.DTOs.Issues;
using CivicIssueTracker.Api.Models;

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
}
