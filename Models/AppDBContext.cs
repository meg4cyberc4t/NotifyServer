using Microsoft.EntityFrameworkCore;

namespace NotifyServer.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<NotifyUser> Users { get; set; } = null!;
    }
}
