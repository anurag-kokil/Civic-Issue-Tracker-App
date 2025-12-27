namespace CivicIssueTracker.Api.Models;

public class IssueStatusHistory
{
    public int Id { get; set; }

    public int IssueId { get; set; }
    public Issue? Issue { get; set; }

    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;

    public int ChangedByUserId { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}