using IdentityService.DataAccess.Abstract.Models;
using IdentityService.Models;
using IdentityService.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Migrations
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserMessageTypeModel> UserMessageTypeTable { get; set; }
        public DbSet<UserMessageModel> UserMessageTable { get; set; }
        public DbSet<AspNetPasswordHistoryModel> AspNetPasswordHistoryTable { get; set; }
        public DbSet<LogModel> LogTable { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.HasDefaultSchema(IdentityServiceConstants.DatabaseSchemaName);
        }
    }
}
