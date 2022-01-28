using NotifyServer.Models;

namespace NotifyServer.Repository;

public interface INotifyNotificationRepository
{
    Task<NotifyNotification?> GetNotificationAsync(Guid id);

    Task<IEnumerable<NotifyNotification>> GetNotificationsAsync(NotifyUser user);

    Task CreateNotificationAsync(NotifyNotification item);

    Task DeleteNotificationAsync(NotifyNotification item);

    Task UpdateNotificationAsync(NotifyNotification items);
}
