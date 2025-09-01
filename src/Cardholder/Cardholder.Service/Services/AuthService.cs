using CardManagement.Data;
using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs;
using CardManagement.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Cardholder.Service.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<CardholderEntity> _cardholderRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IRepository<CardholderEntity> cardholderRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _cardholderRepository = cardholderRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var cardholders = await _cardholderRepository.FindAsync(c => c.Email == request.Email);
            var cardholder = cardholders.FirstOrDefault();

            if (cardholder == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, cardholder.PasswordHash))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var token = await GenerateJwtTokenAsync(cardholder);

            return new LoginResponse
            {
                Token = token,
                User = new UserInfo
                {
                    CardholderId = cardholder.CardholderId,
                    FirstName = cardholder.FirstName,
                    LastName = cardholder.LastName,
                    Email = cardholder.Email,
                    PhoneNumber = cardholder.PhoneNumber,
                    Address = cardholder.Address,
                    CreatedAt = cardholder.CreatedAt,
                    UpdatedAt = cardholder.UpdatedAt
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            throw;
        }
    }

    public async Task<UserInfo> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var existingCardholders = await _cardholderRepository.FindAsync(c => c.Email == request.Email);
            if (existingCardholders.Any())
            {
                throw new ValidationException("Email already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var cardholder = new CardholderEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCardholder = await _cardholderRepository.AddAsync(cardholder);

            return new UserInfo
            {
                CardholderId = createdCardholder.CardholderId,
                FirstName = createdCardholder.FirstName,
                LastName = createdCardholder.LastName,
                Email = createdCardholder.Email,
                PhoneNumber = createdCardholder.PhoneNumber,
                Address = createdCardholder.Address,
                CreatedAt = createdCardholder.CreatedAt,
                UpdatedAt = createdCardholder.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
            throw;
        }
    }

    public async Task<UserInfo> GetUserInfoAsync(int cardholderId)
    {
        var cardholder = await _cardholderRepository.GetByIdAsync(cardholderId);
        if (cardholder == null)
        {
            throw new NotFoundException("Cardholder not found");
        }

        return new UserInfo
        {
            CardholderId = cardholder.CardholderId,
            FirstName = cardholder.FirstName,
            LastName = cardholder.LastName,
            Email = cardholder.Email,
            PhoneNumber = cardholder.PhoneNumber,
            Address = cardholder.Address,
            CreatedAt = cardholder.CreatedAt,
            UpdatedAt = cardholder.UpdatedAt
        };
    }

    public async Task<UserInfo> UpdateUserInfoAsync(int cardholderId, UpdateProfileRequest request)
    {
        var cardholder = await _cardholderRepository.GetByIdAsync(cardholderId);
        if (cardholder == null)
        {
            throw new NotFoundException("Cardholder not found");
        }

        bool hasChanges = false;

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.FirstName) && request.FirstName != cardholder.FirstName)
        {
            cardholder.FirstName = request.FirstName;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(request.LastName) && request.LastName != cardholder.LastName)
        {
            cardholder.LastName = request.LastName;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != cardholder.PhoneNumber)
        {
            cardholder.PhoneNumber = request.PhoneNumber;
            hasChanges = true;
        }

        if (!string.IsNullOrEmpty(request.Address) && request.Address != cardholder.Address)
        {
            cardholder.Address = request.Address;
            hasChanges = true;
        }

        // Handle password change
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (string.IsNullOrEmpty(request.ConfirmNewPassword))
            {
                throw new ValidationException("Confirm password is required when changing password");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                throw new ValidationException("New password and confirm password do not match");
            }

            if (request.NewPassword.Length < 6)
            {
                throw new ValidationException("Password must be at least 6 characters long");
            }

            cardholder.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            hasChanges = true;
        }

        // Only update UpdatedAt if there were actual changes
        if (hasChanges)
        {
            cardholder.UpdatedAt = DateTime.UtcNow;
            await _cardholderRepository.UpdateAsync(cardholder);
        }

        return new UserInfo
        {
            CardholderId = cardholder.CardholderId,
            FirstName = cardholder.FirstName,
            LastName = cardholder.LastName,
            Email = cardholder.Email,
            PhoneNumber = cardholder.PhoneNumber,
            Address = cardholder.Address,
            CreatedAt = cardholder.CreatedAt,
            UpdatedAt = cardholder.UpdatedAt
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured"));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GenerateJwtTokenAsync(CardholderEntity cardholder)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cardholder.CardholderId.ToString()),
                new Claim(ClaimTypes.Email, cardholder.Email),
                new Claim(ClaimTypes.Name, $"{cardholder.FirstName} {cardholder.LastName}")
            }),
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpiryInHours"] ?? "24")),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
} 