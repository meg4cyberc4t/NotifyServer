using Microsoft.EntityFrameworkCore;

namespace NotifyServer.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options){

    }

        public DbSet<NotifyUser> Users { get; set; } = null!;

        protected new void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<NotifyUser>().HasData(
            //     new NotifyUser("localid"));

            modelBuilder.Entity<NotifyUser>().HasKey("Id");
        }
    }
}
