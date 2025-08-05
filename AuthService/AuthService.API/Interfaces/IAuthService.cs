using AuthService.API.DTOs;

namespace AuthService.API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(LogoutDto logoutDto);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
