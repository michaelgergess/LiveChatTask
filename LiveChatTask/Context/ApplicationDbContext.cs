using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using System;

namespace Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatSession>()
           .HasOne(cs => cs.Admin)
           .WithMany()
           .HasForeignKey(cs => cs.AdminId)
           .OnDelete(DeleteBehavior.Restrict); // Configure the delete behavior as needed

            modelBuilder.Entity<ChatSession>()
                .HasOne(cs => cs.User)
                .WithMany()
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Configure the delete behavior as needed


            //Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
