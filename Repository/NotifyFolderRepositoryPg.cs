using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;

namespace NotifyServer.Repository;

public class NotifyFolderRepositoryPg : INotifyFolderRepository
{
    private readonly AppDbContext _context;

    public NotifyFolderRepositoryPg(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NotifyFolder?> GetFolderAsync(Guid id)
    {
        return await _context.Folders
            .Include(e => e.Participants)
            .Include(e => e.NotificationsList)
            .Include(e => e.Creator)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<NotifyFolder>> GetFoldersAsync(NotifyUser inputUser)
    {
        var user = await _context.Users
            .Include(e => e.Folders)
            .ThenInclude(a => a.NotificationsList)
            .Include(e => e.Folders)
            .ThenInclude(a => a.Participants)
            .FirstOrDefaultAsync(e => e.Id == inputUser.Id);
        return user!.Folders;
    }

    public async Task CreateFolderAsync(NotifyFolder folder)
    {
        await _context.Folders.AddAsync(folder);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task DeleteFolderAsync(NotifyFolder folder)
    {
        _context.Folders.Remove(folder);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task UpdateFolderAsync(NotifyFolder folder)
    {
        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }
}
