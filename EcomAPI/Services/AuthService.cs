﻿using AuthAPI.Controllers.Dtos;
using AuthAPI.Interfaces;
using AuthAPI.Models;
using EcomAPI.Interfaces;
using EcomAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            ApplicationDbContext context,
            IUserService userService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
            _userService = userService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var newUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userService.PostUserAsync(newUser);
            
            return await GenerateTokensAsync(newUser);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                throw new Exception("Invalid email or password");

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string id)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.UserId == id && r.IsActive);

            if (tokenEntity == null)
                throw new Exception("Invalid or expired refresh token");

            return await GenerateTokensAsync(tokenEntity.User, true);
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(ApplicationUser user, bool isRefreshFlow = false)
        {
            var accessToken = GenerateAccessToken(user);
            string refreshToken = isRefreshFlow
                ? await ReuseOrRenewRefreshToken(user)
                : await SaveNewRefreshToken(user);

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email
            };
        }
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (token == null)
                return false;

            token.IsRevoked = true;
            
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Revoke token ${token.Token}");
            return true;

        }
        private string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> SaveNewRefreshToken(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            });

            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<string> ReuseOrRenewRefreshToken(ApplicationUser user)
        {
            var existing = await _context.RefreshTokens
                .Where(rt => rt.UserId == user.Id && rt.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(rt => rt.ExpiryDate)
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing.Token;

            return await SaveNewRefreshToken(user);
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("invalid userId");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Cannot find user");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Cannot delete user");
            }
            return result.Succeeded;
        }
        
    }
}
