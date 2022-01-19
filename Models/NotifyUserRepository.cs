using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NotifyServer.Models;

public class NotifyUserRepository : INotifyUserRepository
{
    public NotifyUserRepository(AppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;
    }

    private readonly AppDbContext _appDbContext;

    public DbSet<NotifyUser> Users { get; set; } = null!;

    public async Task<NotifyUser?> Get(string userUid)
    {
        return await _appDbContext.Users
            .FirstOrDefaultAsync(d => d.UserId == userUid);
    }

    public EntityEntry<NotifyUser> Create(NotifyUser user)
    {
        return _appDbContext.Users.Add(user);
    }
}
