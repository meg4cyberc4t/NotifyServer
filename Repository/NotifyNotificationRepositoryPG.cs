using Google.Apis.Util;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyNotificationRepository : INotifyNotificationRepository
{
    private readonly AppDbContext _context;

    public NotifyNotificationRepository(AppDbContext context)
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
            .FirstOrDefaultAsync(e => e.Id == input.Id);
        return user!.Notifications;
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

    // public NotifyNotification? Get(Guid id)
    // {
    //     return _context.Notifications.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
    // }
    //
    // public IEnumerable<NotifyNotification> GetUserNotifications(NotifyUser user)
    // {
    //     return _context.Users
    //         .Include(e => e.Notifications)
    //         .FirstOrDefault(e => e.Id == user.Id)!.Notifications;
    // }
    //
    // public EntityEntry<NotifyNotification> Create(NotifyUser creator, NotifyNotificationInput input)
    // {
    //     var createdNotification = _context.Notifications.Add(new NotifyNotification()
    //     {
    //         Id = Guid.NewGuid(),
    //         Title = input.Title,
    //         Description = input.Description,
    //         Deadline = input.Deadline,
    //         RepeatMode = input.RepeatMode,
    //         Creator = creator,
    //         Important = input.Important,
    //         Participants = new List<NotifyUser>() {creator},
    //         UniqueClaim = new Random().Next()
    //     });
    //     _context.SaveChanges();
    //     return createdNotification;
    // }
    //
    // public void Put(Guid id, NotifyNotificationInput updatedNtf)
    // {
    //     var notification = _context.Notifications.FirstOrDefault(e => e.Id == id)!;
    //     notification.Deadline = updatedNtf.Deadline;
    //     notification.Description = updatedNtf.Description;
    //     notification.Important = updatedNtf.Important;
    //     notification.Title = updatedNtf.Title;
    //     notification.RepeatMode = updatedNtf.RepeatMode;
    //     _context.SaveChanges();
    // }
    //
    // public void Delete(NotifyNotification ntf)
    // {
    //     _context.Notifications.Remove(ntf);
    //     _context.SaveChanges();
    // }
    //
    // public Task<NotifyNotification?> GetNotificationAsync(Guid id)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<IEnumerable<NotifyNotification>> GetNotificationsAsync()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task CreateNotificationAsync(NotifyUser item)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task UpdateNotificationAsync(NotifyUser items)
    // {
    //     throw new NotImplementedException();
    // }
}
