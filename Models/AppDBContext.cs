using Microsoft.EntityFrameworkCore;

namespace NotifyServer.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NotifyUser> Users { get; set; } = null!;
        public DbSet<NotifyNotification> Notifications { get; set; } = null!;
        public DbSet<NotifyFolder> Folders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<NotifyFolder>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.FolderWhereCreator);

            modelBuilder.Entity<NotifyNotification>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.NotificationsWhereCreator);

            base.OnModelCreating(modelBuilder);
        }
    }
}
