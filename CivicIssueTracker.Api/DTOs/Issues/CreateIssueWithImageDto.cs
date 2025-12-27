using Microsoft.AspNetCore.Http;

namespace CivicIssueTracker.Api.DTOs.Issues;

public class CreateIssueWithImageDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public IFormFile? Image { get; set; }
}