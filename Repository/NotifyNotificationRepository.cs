using Google.Apis.Util;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyNotificationRepository
{
    private readonly AppDbContext _context;

    public NotifyNotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public NotifyNotification? Get(Guid id)
    {
        return _context.Notifications.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<NotifyNotification> GetUserNotifications(NotifyUser user)
    {
        return _context.Users
            .Include(e => e.Notifications)
            .FirstOrDefault(e => e.Id == user.Id)!.Notifications;
    }

    public EntityEntry<NotifyNotification> Create(NotifyUser creator, NotifyNotificationInput input)
    {
        var createdNotification = _context.Notifications.Add(new NotifyNotification()
        {
            Id = Guid.NewGuid(),
            Title = input.Title,
            Description = input.Description,
            Deadline = input.Deadline,
            RepeatMode = input.RepeatMode,
            Creator = creator,
            Important = input.Important,
            Participants = new List<NotifyUser>() {creator},
            UniqueClaim = new Random().Next()
        });
        _context.SaveChanges();
        return createdNotification;
    }

    // public void Put(IQueryable<NotifyNotification> notification, NotifyNotificationInput input)
    // {
    //     notification.Title = input.Title;
    //     notification.Description = input.Description;
    //     notification.Deadline = input.Deadline;
    //     notification.RepeatMode = input.RepeatMode;
    //     notification.Important = input.Important;
    //     _context.SaveChanges();
    // }

    public void Delete(NotifyNotification ntf)
    {
        _context.Notifications.Remove(ntf);
        _context.SaveChanges();
    }
}
