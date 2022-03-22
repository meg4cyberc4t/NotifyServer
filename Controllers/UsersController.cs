using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("users/{id:guid}")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly INotifyUserRepository _users;

    public UsersController(AppDbContext context)
    {
        _users = new NotifyUserReposoitoryPg(context);
    }

    [HttpGet(Name = "GetUserById")]
    public async Task<ActionResult<NotifyUserDetailed>> Get(Guid id)
    {
        var requestUser = (HttpContext.Items["User"] as NotifyUser)!;
        var user = await _users.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToNotifyUserDetailed(requestUser));
    }

    [HttpGet("subscriptions")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> SubscriptionsById(Guid id)
    {
        var user = await _users.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Subscriptions.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpGet("subscribers")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> SubscribersById(Guid id)
    {
        var user = await _users.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Subscribers.Select(e => e.ToNotifyUserQuick()));
    }
    
    [HttpGet("notifications")]
    public async Task<ActionResult<IEnumerable<NotifyNotificationQuick>>> NotificationById(Guid id)
    {
        var requestUser = (HttpContext.Items["User"] as NotifyUser)!;

        var user = await _users.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Notifications.Intersect(requestUser.Notifications).Select(e => e.ToNotifyNotificationQuick()));
    }
}
