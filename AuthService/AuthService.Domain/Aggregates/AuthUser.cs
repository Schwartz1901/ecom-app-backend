using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.Entities;
using AuthService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Aggregates
{
    public class AuthUser : Entity, IAggregateRoot
    {
        public AuthId Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private AuthUser() { } // for EF Core

        public AuthUser( string username, string passwordHash, string role = "User")
        {
            Id = AuthId.New();
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
        
        public void AddRefreshToken(string token, DateTime expiresAt)
        {
            var refreshToken = new RefreshToken(token, expiresAt);
            _refreshTokens.Add(refreshToken);
        }

        public void RevokeAllTokens()
        {
            foreach (var token in _refreshTokens)
            {
                token.Revoke();
            }
        }
    }

}
