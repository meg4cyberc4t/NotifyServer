using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NotifyServer.Models;

public interface INotifyUserRepository
{
    Task<NotifyUser?> Get(string userUid);
    EntityEntry<NotifyUser> Create(NotifyUser user);
}
