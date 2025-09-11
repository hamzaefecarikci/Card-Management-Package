using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Models;
using Cardholder.Service.Services;
using System.Security.Claims;

namespace Cardholder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new cardholder
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>User information</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<UserInfo>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(ApiResponse<UserInfo>.SuccessResult(result, "User registered successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(ApiResponse<LoginResponse>.SuccessResult(result, "Login successful"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            return Unauthorized(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    /// <returns>User profile information</returns>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserInfo>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 401)]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            var result = await _authService.GetUserInfoAsync(cardholderId);
            return Ok(ApiResponse<UserInfo>.SuccessResult(result, "Profile retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving profile");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Update user profile (partial update)
    /// </summary>
    /// <param name="request">Profile update request</param>
    /// <returns>Updated user information</returns>
    [HttpPut("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserInfo>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        try
        {
            var cardholderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (cardholderId == 0)
            {
                return Unauthorized(ApiResponse<object>.ErrorResult("Invalid token"));
            }

            var result = await _authService.UpdateUserInfoAsync(cardholderId, request);
            return Ok(ApiResponse<UserInfo>.SuccessResult(result, "Profile updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            return BadRequest(ApiResponse<object>.ErrorResult(ex.Message));
        }
    }
} 