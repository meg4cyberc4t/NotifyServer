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
    private readonly NotifyNotificationRepository _notificationRepository;

    public NotificationController(AppDbContext context)
    {
        _notificationRepository = new NotifyNotificationRepository(context);
    }

    [HttpGet]
    public ActionResult<IEnumerable<NotifyNotificationDetailed>> GetMyNotifications()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(_notificationRepository.GetUserNotifications(user).Select(e => e.ToNotifyNotificationDetailed()));
    }

    [HttpGet("{id:guid}", Name = "GetNotificationById")]
    public ActionResult<NotifyNotificationDetailed> Get(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var ntf = _notificationRepository.Get(id);
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

    [HttpPost]
    public ActionResult<NotifyNotificationDetailed> Create([FromBody] NotifyNotificationInput input)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(_notificationRepository.Create(user, input).Entity.ToNotifyNotificationDetailed());
    }

    [HttpDelete("{id:guid}", Name = "DeleteNotificationById")]
    public ActionResult Delete(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        var notification = _notificationRepository.Get(id);
        if (notification == null)
        {
            return NotFound();
        }

        if (!notification.Participants.Contains(user))
        {
            return Forbid();
        }

        _notificationRepository.Delete(notification);
        return NoContent();
    }
}
