using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using System;
using System.Reflection.Emit;

namespace NetCoreAPI
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<EmailCode> EmailCode { get; set; }
        public DbSet<Friendship> FriendShip { get; set; }
        public DbSet<Solicitation> Solicitation { get; set; }
        public DbSet<UserServer> UserServer { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<MessageChannel> MessageChannel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            builder.Entity<UserServer>()
            .HasKey(us => new { us.UserId, us.ServerId });

            builder.Entity<UserServer>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserServers)
                .HasForeignKey(us => us.UserId)
            ;

            builder.Entity<UserServer>()
                .HasOne(us => us.Server)
                .WithMany(s => s.UserServers)
                .HasForeignKey(us => us.ServerId)
            ;

            builder.Entity<Server>()
       .HasOne(s => s.Admin)
       .WithMany()
       .HasForeignKey(s => s.AdminId)
       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
       .HasKey(c => c.Id);

            builder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId);

            builder.Entity<User>()
                .HasMany(u => u.Conversations)
                .WithOne(c => c.FirstUser)
                .HasForeignKey(c => c.FirstUserId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
            .HasKey(a => new { a.FirstUserId, a.SecondUserId });

            builder.Entity<Friendship>()
                .HasOne(a => a.FirstUser)
                .WithMany(u => u.Friends)
                .HasForeignKey(a => a.FirstUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
                .HasOne(a => a.SecondUser)
                .WithMany() 
                .HasForeignKey(a => a.SecondUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Solicitation>()
            .HasKey(a => new { a.UserId, a.SecondUserId });

            builder.Entity<Solicitation>()
                .HasOne(a => a.User)
                .WithMany(u => u.Solicitation)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Solicitation>()
                .HasOne(a => a.SecondUser)
                .WithMany()
                .HasForeignKey(a => a.SecondUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Channel>()
          .HasOne(c => c.Server)
          .WithMany(s => s.Channels)
          .HasForeignKey(c => c.ServerId);

            builder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.NoAction);// Evita ação de exclusão em cascata

            builder.Entity<Channel>()
        .HasMany(c => c.MessageChannel) // Um canal tem muitas mensagens
        .WithOne(mc => mc.Channel)      // Cada mensagem pertence a um único canal
        .HasForeignKey(mc => mc.ChannelId);
        }
    }

}
