using LiveChatTaskMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.MessagesAsSender)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.MessagesAsReciver)
                .WithOne(m => m.Reciver)
                .HasForeignKey(m => m.ReciverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationships for FileAttachments
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.FileAttachmentsAsSender)
                .WithOne(f => f.Sender)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.FileAttachmentsAsReciver)
                .WithOne(f => f.Reciver)
                .HasForeignKey(f => f.ReciverId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
