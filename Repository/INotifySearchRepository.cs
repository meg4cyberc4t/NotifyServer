using NotifyServer.Models;

namespace NotifyServer.Repository;

public interface INotifySearchRepository
{
    IQueryable<NotifyUser> FromUsersQuery(string pattern, int limit, int offset);
    IQueryable<NotifyNotification> FromNotificationsQuery(string pattern, int limit, int offset);
    IQueryable<NotifyFolder> FromFoldersQuery(string pattern, int limit, int offset);

}
