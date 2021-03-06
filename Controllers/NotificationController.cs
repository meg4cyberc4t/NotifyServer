using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("notifications")]
[ApiController]
public class NotificationController : Controller
{
    private readonly INotifyNotificationRepository _notificationRepository;
    private readonly INotifyUserRepository _userRepository;

    public NotificationController(AppDbContext context)
    {
        _notificationRepository = new NotifyNotificationRepositoryPg(context);
        _userRepository = new NotifyUserReposoitoryPg(context);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotifyNotificationDetailed>>> GetMyNotifications()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var ntfs = await _notificationRepository.GetNotificationsAsync(user);
        return Ok(ntfs.Select(e => e.ToNotifyNotificationQuick()));
    }

    [HttpGet("{id:guid}/participants", Name = "GetParticipantsById")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> GetParticipantsById(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var ntf = await _notificationRepository.GetNotificationAsync(id);
        if (ntf == null)
        {
            return NotFound();
        }

        if (!ntf.Participants.Contains(user))
        {
            return Forbid();
        }

        return Ok(ntf.Participants.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpPost]
    public async Task<ActionResult<NotifyNotificationDetailed>> Create([FromBody] NotifyNotificationInput input)
    {      

        var user = (HttpContext.Items["User"] as NotifyUser)!;
        
        var ntf = new NotifyNotification()
        {
            Id = Guid.NewGuid(),
            Title = input.Title,
            Description = input.Description,
            Deadline = DateTime.SpecifyKind(input.Deadline, DateTimeKind.Utc),
            RepeatMode = input.RepeatMode,
            Creator = user,
            Important = input.Important,
            Participants = new List<NotifyUser>() {user},
            UniqueClaim = new Random().Next(),
        };
        await _notificationRepository.CreateNotificationAsync(ntf);
        return Ok(ntf.ToNotifyNotificationDetailed());
    }

    [HttpDelete("{id:guid}", Name = "DeleteNotificationById")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var notification = await _notificationRepository.GetNotificationAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        if (!notification.Participants.Contains(user))
        {
            return Forbid();
        }

        if (notification.Creator.Id != user.Id)
        {
            notification.Participants.Remove(user);
            await _notificationRepository.UpdateNotificationAsync(notification);
            return NoContent();
        }
        
        await _notificationRepository.DeleteNotificationAsync(notification);
        return NoContent();
    }

    [HttpGet("{id:guid}", Name = "GetNotificationById")]
    public async Task<ActionResult<NotifyNotificationDetailed>> Get(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var ntf = await _notificationRepository.GetNotificationAsync(id);
        if (ntf == null)
        {
            return NotFound();
        }

        if (!ntf.Participants.Contains(user))
        {
            return Forbid();
        }

        return Ok(ntf.ToNotifyNotificationDetailed());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<NotifyNotificationDetailed>> Put(Guid id,
        [FromBody] NotifyNotificationInput updatedNtf)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var notification = await _notificationRepository.GetNotificationAsync(id: id);
        if (notification == null)
        {
            return NotFound();
        }

        if (!notification.Participants.Contains(user))
        {
            return Forbid();
        }

        notification.Deadline = DateTime.SpecifyKind(updatedNtf.Deadline, DateTimeKind.Utc);
        notification.Title = updatedNtf.Title;
        notification.Description = updatedNtf.Description;
        notification.RepeatMode = updatedNtf.RepeatMode;
        notification.Important = updatedNtf.Important;
        await _notificationRepository.UpdateNotificationAsync(notification);
        return Ok(notification.ToNotifyNotificationDetailed());
    }

    [HttpPost("{id:guid}/invite")]
    public async Task<ActionResult> Invite(Guid id, [FromQuery][Required] Guid inviteUserId)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var notification = await _notificationRepository.GetNotificationAsync(id: id);
        var inviteUser = await _userRepository.GetUserAsync(inviteUserId);

        if (notification == null || inviteUser == null)
        {
            return BadRequest();
        }

        if (inviteUser.Id == user.Id)
        {
            return BadRequest();
        }

        if (!(notification.Participants.Contains(user) || notification.Creator == user ||
              !user.Subscribers.Contains(inviteUser)))
        {
            return Forbid();
        }

        if (!notification.Participants.Contains(inviteUser))
        {
            notification.Participants.Add(inviteUser);
        }

        await _notificationRepository.UpdateNotificationAsync(notification);
        return NoContent();
    }

    [HttpPost("{id:guid}/exclude")]
    public async Task<ActionResult> Exclude(Guid id, [FromQuery][Required] Guid excludeUserId)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var notification = await _notificationRepository.GetNotificationAsync(id: id);
        var inviteUser = await _userRepository.GetUserAsync(excludeUserId);

        if (notification == null || inviteUser == null)
        {
            return BadRequest();
        }

        if (inviteUser.Id == user.Id)
        {
            return BadRequest();
        }
        
        if (!(notification.Participants.Contains(user) || notification.Creator == user))
        {
            return Forbid();
        }

        if (notification.Participants.Contains(inviteUser))
        {
            notification.Participants.Remove(inviteUser);
        }

        await _notificationRepository.UpdateNotificationAsync(notification);

        return NoContent();
    }
}
