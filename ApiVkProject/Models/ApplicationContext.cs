using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace ApiVkProject.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupHasUser> GroupHasUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupHasUser>().HasKey(t => new { t.GroupId, t.UserId });

            modelBuilder.Entity<GroupHasUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupHasUser)
                .HasForeignKey(gu => gu.GroupId);

            modelBuilder.Entity<GroupHasUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupHasUser)
                .HasForeignKey(gu => gu.UserId);
        }
    }
}
