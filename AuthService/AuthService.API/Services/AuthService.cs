using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using AuthService.Domain.Aggregates;
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
            {
                throw new Exception("User is already existed");
            }
            var userProfileClient = _httpClientFactory.CreateClient("UserService");
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newUser = new AuthUser(request.Username, passwordHash, "User");

               
               var response = await userProfileClient.PostAsJsonAsync("User", new
                {
                    ID = newUser.Id.ToString(),
                    Username = request.Username,
                    Mail = request.Email,
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
                var token = GenerateAccessToken(newUser);
                var refreshToken = GenerateRefreshToken();
                newUser.AddRefreshToken(refreshToken.Token, refreshToken.ExpiresAt);

                await _authUserRepository.AddAsync(newUser);

                await _unitOfWork.CommitAsync();

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
               await  _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            return new AuthResponseDto
            {
                Token = "",
                RefreshToken = "",
                AuthId = "",
                Email = "",
                Username = "",
            };
        }

        public Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
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
