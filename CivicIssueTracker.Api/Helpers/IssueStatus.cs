namespace CivicIssueTracker.Api.Helpers;

public static class IssueStatus
{
    public const string Reported = "Reported";
    public const string Assigned = "Assigned";
    public const string InProgress = "InProgress";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";

    public static readonly Dictionary<string, string[]> AllowedTransitions =
        new()
        {
            { Reported, new[] { Assigned } },
            { Assigned, new[] { InProgress } },
            { InProgress, new[] { Resolved } },
            { Resolved, new[] { Closed } }
        };
}