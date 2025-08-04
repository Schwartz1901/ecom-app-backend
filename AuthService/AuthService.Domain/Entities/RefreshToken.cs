using AuthService.Domain.Aggregates;
using AuthService.Domain.Aggregates.ValueObjects;
using AuthService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public TokenId Id { get; private set; }
        public AuthId AuthId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }

        public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;

        public RefreshToken(string token, DateTime expiresAt)
        {
            Id = TokenId.NewId();
            Token = token;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public void Revoke() => IsRevoked = true;
    }
}
