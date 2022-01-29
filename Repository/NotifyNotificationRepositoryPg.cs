using System.Reflection.Metadata;
using Google.Apis.Util;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyNotificationRepositoryPg : INotifyNotificationRepository
{
    private readonly AppDbContext _context;

    public NotifyNotificationRepositoryPg(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotifyNotification?> GetNotificationAsync(Guid id)
    {
        return await _context.Notifications
            .Include(e => e.Participants)
            .Include(e => e.Creator)
            .FirstOrDefaultAsync(e => e.Id == id);
    }


    public async Task<IEnumerable<NotifyNotification>> GetNotificationsAsync(NotifyUser input)
    {
        var user = await _context.Users
            .Include(e => e.Notifications)
            .ThenInclude(e => e.Creator)
            .FirstOrDefaultAsync(e => e.Id == input.Id);
        return user!.Notifications;
    }

    public async Task<IEnumerable<NotifyNotification>> GetNotificationsFromIdsListAsync(List<Guid> ids)
    {
        var ntfs = await _context.Notifications.Where(e => ids.Contains(e.Id)).ToListAsync();
        return ntfs;
    }

    public async Task CreateNotificationAsync(NotifyNotification ntf)
    {
        await _context.Notifications.AddAsync(ntf);
        await Task.CompletedTask;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNotificationAsync(NotifyNotification ntf)
    {
        _context.Notifications.Update(ntf);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(NotifyNotification ntf)
    {
        _context.Notifications.Remove(ntf);
        await _context.SaveChangesAsync();
    }
}
