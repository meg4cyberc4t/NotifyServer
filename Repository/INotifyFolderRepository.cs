using NotifyServer.Models;

namespace NotifyServer.Repository;

public interface INotifyFolderRepository
{
    Task<NotifyFolder?> GetFolderAsync(Guid id);

    Task<IEnumerable<NotifyFolder>> GetFoldersAsync(NotifyUser user);

    Task CreateFolderAsync(NotifyFolder folder);

    Task DeleteFolderAsync(NotifyFolder folder);

    Task UpdateFolderAsync(NotifyFolder folder);
}
