using ChessPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChessPlatform.Data
{
    public class ChessPlatformContext : IdentityDbContext<ApplicationUser>
    {
        public ChessPlatformContext(DbContextOptions<ChessPlatformContext> options) : base(options) { }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TournamentPlayer>()
                .HasOne(tp => tp.Tournament)
                .WithMany()
                .HasForeignKey(tp => tp.TournamentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TournamentPlayer>()
                .HasOne(tp => tp.Player)
                .WithMany()
                .HasForeignKey(tp => tp.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TournamentPlayer>()
                   .HasIndex(tp => new { tp.TournamentId, tp.PlayerId })
                   .IsUnique();
            builder.Entity<Match>()
                    .HasOne(m => m.Player1)
                    .WithMany()
                    .HasForeignKey(m => m.Player1Id)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    } 
}
