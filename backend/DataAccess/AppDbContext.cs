using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Utilities.Security;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
           : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Chat> Chats{ get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Veritabanı tablolarının ve ilişkilerin nasıl oluşturulacağını tanımlamak için kullanılır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global Query Filter
            modelBuilder.Entity<Chat>()
                .HasQueryFilter(c => _currentUserService.Role == "Admin" || c.OwnerUserId == _currentUserService.UserId);

            modelBuilder.Entity<Message>()
                .HasQueryFilter(m => _currentUserService.Role == "Admin" || m.OwnerUserId == _currentUserService.UserId);

            modelBuilder.Entity<Response>()
                .HasQueryFilter(r => _currentUserService.Role == "Admin" || r.OwnerUserId == _currentUserService.UserId);


            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade); // OnDelete(DeleteBehavior.Cascade) Entity Framework Core’da ilişki silme davranışını belirler
                                                   // Cascade, Restrict, SetNull, NoAction gibi seçenekler içerir.

            modelBuilder.Entity<Response>()
                .HasOne(r => r.Message)
                .WithOne(m => m.Response)
                .HasForeignKey<Response>(r => r.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

              // User -> Chat
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.Chats)
                .HasForeignKey(c => c.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Owner)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Response
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Responses)
                .HasForeignKey(r => r.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
