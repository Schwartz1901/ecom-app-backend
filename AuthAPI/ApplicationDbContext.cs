﻿using AuthAPI.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
