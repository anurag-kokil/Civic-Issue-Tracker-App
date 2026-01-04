using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CivicIssueTracker.Api.Models
{
    public class Issue
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Status { get; set; } = "Reported";

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string? ImageUrl { get; set; }

        public int ReportedByUserId { get; set; }

        public User? ReportedByUser { get; set; }

        public int? AssignedOfficerId { get; set; }

        public User? AssignedOfficer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}