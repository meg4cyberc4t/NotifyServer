using Google.Apis.Util;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyUserRepository
{
    private readonly AppDbContext _context;

    public NotifyUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public NotifyUser? Get(Guid id)
    {
        return _context.Users.Include(e => e.Subscribers).Include(e => e.Subscriptions)
            .FirstOrDefault(e => e.Id == id);
    }

    public List<NotifyUser> GetAll()
    {
        return _context.Users.ToList();
    }

    public EntityEntry<NotifyUser> Create(NotifyUserInput user)
    {
        EntityEntry<NotifyUser> createdUser = _context.Users.Add(new NotifyUser
        {
            Id = Guid.NewGuid(),
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Color = user.Color,
            Subscribers = new List<NotifyUser>(),
            Subscriptions = new List<NotifyUser>(),
        });
        _context.SaveChanges();
        return createdUser;
    }

    public void Put(NotifyUserQuick updates)
    {
        NotifyUser? user = _context.Users.First(e => e.Id == updates.Id);
        user.Color = updates.Color;
        user.Firstname = updates.Firstname;
        user.Lastname = updates.Lastname;
        _context.SaveChanges();
    }

    public void ChangeSubscription(Guid fromId, Guid toId)
    {
        NotifyUser? fromUser = _context.Users.Include(e => e.Subscribers)
            .FirstOrDefault(e => e.Id == fromId);
        NotifyUser? toUser = _context.Users.Include(e => e.Subscriptions)
            .FirstOrDefault(e => e.Id == toId);
        if (fromUser == null || toUser == null)
        {
            return;
        }
        if (fromUser.Subscribers.Contains(toUser) || toUser.Subscriptions.Contains(fromUser))
        {
            toUser.Subscriptions.Remove(fromUser);
            fromUser.Subscribers.Remove(fromUser);
        }
        else
        {
            toUser.Subscriptions.Add(fromUser);
            fromUser.Subscribers.Add(fromUser);
        }
        _context.SaveChanges();
    }
}