using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifySearchRepositoryPg : INotifySearchRepository
{
    private readonly AppDbContext _context;

    public NotifySearchRepositoryPg(AppDbContext context)
    {
        _context = context;

    }


    public IQueryable<NotifyUser> FromUsersQuery(string pattern, int limit, int offset)
    {
        return _context.Users.Where(e => e.Firstname.Contains(pattern) || e.Lastname.Contains(pattern)).Skip(offset).Take(limit);
    }

    public IQueryable<NotifyNotification> FromNotificationsQuery(string pattern, int limit, int offset)
    {
        return _context.Notifications.Where(e => e.Title.Contains(pattern) || e.Description.Contains(pattern)).Skip(offset).Take(limit);
    }
    public IQueryable<NotifyFolder> FromFoldersQuery(string pattern, int limit, int offset)
    {
        return _context.Folders.Where(e => e.Title.Contains(pattern) || e.Description.Contains(pattern)).Skip(offset).Take(limit);
    }
}
