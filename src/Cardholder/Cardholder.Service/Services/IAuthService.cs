using CardManagement.Data.Entities;
using CardManagement.Shared.DTOs;

namespace Cardholder.Service.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<UserInfo> RegisterAsync(RegisterRequest request);
    Task<UserInfo> GetUserInfoAsync(int cardholderId);
    Task<UserInfo> UpdateUserInfoAsync(int cardholderId, UpdateProfileRequest request);
    Task<bool> ValidateTokenAsync(string token);
    Task<string> GenerateJwtTokenAsync(CardholderEntity cardholder);
} 