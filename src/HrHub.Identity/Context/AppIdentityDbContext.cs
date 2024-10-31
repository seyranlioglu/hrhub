using HrHub.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Context
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, long, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        //public virtual DbSet<AppUser> Users { get; set; }
        //public virtual DbSet<AppRole> Roles { get; set; }
        //public virtual DbSet<AppUserClaim> UserClaims { get; set; }
        //public virtual DbSet<AppUserRole> UserRoles { get; set; }
        //public virtual DbSet<AppUserLogin> UserLogins { get; set; }
        //public virtual DbSet<AppRoleClaim> RoleClaims { get; set; }
        //public virtual DbSet<AppUserToken> UserTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<AppRole>().ToTable("Roles");
            builder.Entity<AppUserClaim>().ToTable("UserClaims");
            builder.Entity<AppUserRole>().ToTable("UserRoles");
            builder.Entity<AppUserLogin>().ToTable("UserLogins");
            builder.Entity<AppRoleClaim>().ToTable("RoleClaims");
            builder.Entity<AppUserToken>().ToTable("UserTokens");

            base.OnModelCreating(builder);
        }
    }
}
