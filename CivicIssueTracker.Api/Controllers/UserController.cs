using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CivicIssueTracker.Api.Models;
using CivicIssueTracker.Api.Data;

namespace CivicIssueTracker.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("officers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOfficers()
        {
            var officers = await _context.Users
                .Where(u => u.Role == "Officer")
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email
                })
                .ToListAsync();

            return Ok(officers);
        }
    }
}