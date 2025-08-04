using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Aggregates.ValueObjects;
using User.Domain.SeedWork;

namespace User.Domain.Aggregates
{
    public class UserAggregate : IAggregateRoot
    {
        public UserId Id { get; private set; }
        public Guid AuthId { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; private set; }

        // Optional: domain-specific profile fields
        public string? PhoneNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private UserAggregate() { }
        public UserAggregate( Guid authId, string username, string email)
        {
            Id = UserId.NewId();
            AuthId = authId;
            Username = username;
            Email = email;
            
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateProfile(string username, string email, string? phoneNumber)
        {
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
        }

    }
}
