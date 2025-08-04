using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using AuthService.Domain.Aggregates;
using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.SeedWork;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AuthService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthUserRepository _authUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        public AuthService(IAuthUserRepository authUserRepository,
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _authUserRepository = authUserRepository;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            var exists = await _authUserRepository.GetByName(request.Username);
            if (exists != null)
                throw new Exception("User is already existed");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new AuthUser(request.Username, request.Email, passwordHash, "User");

            var userProfileClient = _httpClientFactory.CreateClient("UserService");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _authUserRepository.AddAsync(newUser);
                var response = await userProfileClient.PostAsJsonAsync("User", new
                {
                    ID = newUser.Id.ToString(),
                    Username = request.Username,
                    Mail = request.Email,
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to create user profile: {error}");
                }

                var refreshToken = GenerateRefreshToken();
                newUser.AddRefreshToken(refreshToken.Token, refreshToken.ExpiresAt);

                await _unitOfWork.CommitAsync();

                var token = GenerateAccessToken(newUser);

                return new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken.Token,
                    AuthId = newUser.Id.ToString(),
                    Username = newUser.Username,
                    Email = request.Email,
                };
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }
            


        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            // Check if user exists
            var user = await _authUserRepository.GetByName(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            // Generate tokens
            var token = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            // Add refresh token to user aggregate
            user.AddRefreshToken(refreshToken.Token, refreshToken.ExpiresAt);

            // Commit changes
            await _unitOfWork.CommitAsync();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                AuthId = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<bool> LogoutAsync(Guid authId, string refreshToken)
        {
            var id = new AuthId(authId);
            var user = await _authUserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.RevokeToken(refreshToken);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var id = new AuthId(userId);
            var user = await _authUserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var userProfileClient = _httpClientFactory.CreateClient("UserService");

            // Start a DB transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Delete user from Auth DB
                await _authUserRepository.RemoveAsync(id);

                // Call UserService to delete profile
                var response = await userProfileClient.DeleteAsync($"User/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to delete profile from UserService: {error}");
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Find the user that owns this token
            var user = await _authUserRepository.GetByRefreshToken(refreshToken);
            if (user == null)
                throw new Exception("Invalid refresh token");

            var tokenEntity = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
            if (tokenEntity == null || !tokenEntity.IsActive)
                throw new Exception("Refresh token is expired or revoked");

            // Revoke the old token (rotation)
            tokenEntity.Revoke();

            // Generate new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.AddRefreshToken(newRefreshToken.Token, newRefreshToken.ExpiresAt);

            await _unitOfWork.CommitAsync();

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                AuthId = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email
            };
        }

        /// <summary>
        /// Token Related Method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateAccessToken(AuthUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            return new RefreshToken(
                token: token,
                expiresAt: DateTime.UtcNow.AddDays(7)
            );
        }


    }
}
