using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("folders")]
[ApiController]
public class FolderController : Controller
{
    private readonly INotifyNotificationRepository _notificationRepository;
    private readonly INotifyUserRepository _userRepository;
    private readonly INotifyFolderRepository _folderRepository;

    public FolderController(AppDbContext context)
    {
        _userRepository = new NotifyUserReposoitoryPg(context);
        _notificationRepository = new NotifyNotificationRepositoryPg(context);
        _folderRepository = new NotifyFolderRepositoryPg(context);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotifyFolderDetailed>>> GetMyFolders()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folders = await _folderRepository.GetFoldersAsync(user);
        return Ok(folders.Select(e => e.ToNotifyFolderDetailed()));
    }


    [HttpGet("{id:guid}", Name = "GetFolderById")]
    public async Task<ActionResult<NotifyFolderDetailed>> Get(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id);
        if (folder == null)
        {
            return NotFound();
        }

        if (!folder.Participants.Contains(user))
        {
            return Forbid();
        }

        return Ok(folder.ToNotifyFolderDetailed());
    }

    [HttpGet("{id:guid}/notifications", Name = "GetNotificationsByFolder")]
    public async Task<ActionResult<IEnumerable<NotifyNotificationDetailed>>> GetNotificationsByFolder(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id);
        if (folder == null)
        {
            return NotFound();
        }

        if (!folder.Participants.Contains(user))
        {
            return Forbid();
        }

        return Ok(folder.NotificationsList.Select(e => e.ToNotifyNotificationDetailed()));
    }

    [HttpGet("{id:guid}/participants", Name = "GetParticipantsByFolder")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> GetParticipantsByFolder(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id);
        if (folder == null)
        {
            return NotFound();
        }

        if (!folder.Participants.Contains(user))
        {
            return Forbid();
        }

        return Ok(folder.Participants.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpPost]
    public async Task<ActionResult<NotifyFolderDetailed>> Create([FromBody] NotifyFolderInput input)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = new NotifyFolder()
        {
            Id = Guid.NewGuid(),
            Title = input.Title,
            Description = input.Description,
            Creator = user,
            Participants = new List<NotifyUser>() {user},
            NotificationsList = new List<NotifyNotification>(),
        };
        await _folderRepository.CreateFolderAsync(folder);
        return Ok(folder.ToNotifyFolderDetailed());
    }

    [HttpDelete("{id:guid}", Name = "DeleteFolderById")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id);
        if (folder == null)
        {
            return NotFound();
        }

        if (!folder.Participants.Contains(user))
        {
            return Forbid();
        }

        await _folderRepository.DeleteFolderAsync(folder);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<NotifyFolderDetailed>> Put(Guid id,
        [FromBody] NotifyFolderInput updatedFolder)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id: id);
        if (folder == null)
        {
            return NotFound();
        }

        if (!folder.Participants.Contains(user))
        {
            return Forbid();
        }

        folder.Title = updatedFolder.Title;
        folder.Description = updatedFolder.Description;
        await _folderRepository.UpdateFolderAsync(folder);
        return Ok(folder.ToNotifyFolderDetailed());
    }

    [HttpPost("{id:guid}/invite")]
    public async Task<ActionResult> Invite(Guid id, [FromQuery] Guid inviteUserId)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id: id);
        var inviteUser = await _userRepository.GetUserAsync(inviteUserId);

        if (folder == null || inviteUser == null)
        {
            return BadRequest();
        }

        if (!(folder.Participants.Contains(user) || folder.Creator == user || !user.Subscribers.Contains(inviteUser)))
        {
            return Forbid();
        }

        if (folder.Participants.Contains(inviteUser))
        {
            return BadRequest();
        }

        folder.Participants.Add(inviteUser);
        await _folderRepository.UpdateFolderAsync(folder);
        return NoContent();
    }

    [HttpPost("{id:guid}/exclude")]
    public async Task<ActionResult> Exclude(Guid id, [FromQuery] Guid excludeUserId)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var folder = await _folderRepository.GetFolderAsync(id: id);
        var inviteUser = await _userRepository.GetUserAsync(excludeUserId);

        if (folder == null || inviteUser == null)
        {
            return BadRequest();
        }

        if (!(folder.Participants.Contains(user) || folder.Creator == user))
        {
            return Forbid();
        }


        if (!folder.Participants.Contains(inviteUser))
        {
            return BadRequest();
        }

        folder.Participants.Remove(inviteUser);
        await _folderRepository.UpdateFolderAsync(folder);

        return NoContent();
    }
}
