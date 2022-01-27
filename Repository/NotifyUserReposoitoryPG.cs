using FirebaseAdmin.Auth.Multitenancy;
using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyUserReposoitory : INotifyUserRepository
{
    private readonly AppDbContext _context;

    public NotifyUserReposoitory(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotifyUser?> GetUserAsync(Guid id)
    {
        return await _context.Users
            .Include(e => e.Subscribers)
            .Include(e => e.Subscriptions)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<NotifyUser?> GetUserByForgeinUidAsync(string uid)
    {
        return await _context.Users
            .Include(e => e.Subscribers)
            .Include(e => e.Subscriptions)
            .FirstOrDefaultAsync(e => e.ForgeinUid == uid);
    }

    public async Task<IEnumerable<NotifyUser>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task CreateUserAsync(NotifyUser user)
    {
        await _context.Users.AddAsync(user);
        await Task.CompletedTask;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(NotifyUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
