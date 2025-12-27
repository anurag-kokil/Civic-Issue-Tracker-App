using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CivicIssueTracker.Api.Data;
using CivicIssueTracker.Api.DTOs.Auth;
using CivicIssueTracker.Api.Helpers;
using CivicIssueTracker.Api.Models;

namespace CivicIssueTracker.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenHelper _jwtHelper;

    public AuthController(AppDbContext context, JwtTokenHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null ||
            !PasswordHasher.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _jwtHelper.GenerateToken(user);

        return Ok(new { token });
    }
}
