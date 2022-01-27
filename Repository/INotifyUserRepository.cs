using NotifyServer.Models;

namespace NotifyServer.Repository;

public interface INotifyUserRepository
{
    Task<NotifyUser?> GetUserAsync(Guid id);
    Task<NotifyUser?> GetUserByForgeinUidAsync(string id);

    Task<IEnumerable<NotifyUser>> GetUsersAsync();
    Task CreateUserAsync(NotifyUser item);

    Task UpdateUserAsync(NotifyUser items);
}
