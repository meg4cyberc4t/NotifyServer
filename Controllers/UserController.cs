using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly INotifyUserRepository _users;

    public UserController(AppDbContext context)
    {
        _users = new NotifyUserReposoitoryPg(context);
    }

    [HttpGet]
    public NotifyUserDetailed Get()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        
        return user.ToNotifyUserDetailed(user);
    }

    [HttpPost]
    public async Task<ActionResult<NotifyUserDetailed>> Create([FromBody] NotifyUserInput input)
    {
        // This method is specifically not found in middleware, in order to create a user.
        // It is important to take the uid here
        var uid = HttpContext.User.Claims.First(e => e.Type == "user_id").Value;
        var user = await _users.GetUserByForgeinUidAsync(uid);
        if (user != null) return Conflict();
        var newUser = new NotifyUser
        {
            Id = Guid.NewGuid(),
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Color = input.Color,
            Status = input.Status,
            Subscribers = new List<NotifyUser>(),
            Subscriptions = new List<NotifyUser>(),
            ForgeinUid = uid
        };
        await _users.CreateUserAsync(newUser);
        return Ok(newUser.ToNotifyUserDetailed(user!));
    }

    [HttpPut]
    public async Task<ActionResult<NotifyUserQuick>> Put([FromBody] NotifyUserInput updatedUser)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        user.Color = updatedUser.Color;
        user.Firstname = updatedUser.Firstname;
        user.Lastname = updatedUser.Lastname;
        user.Status = updatedUser.Status;
        await _users.UpdateUserAsync(user);
        return Ok(user.ToNotifyUserDetailed(user));
    }

    [HttpGet("/subscriptions")]
    public ActionResult<IEnumerable<NotifyUserQuick>> Subscriptions()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(user.Subscriptions.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpGet("/subscribers")]
    public ActionResult<IEnumerable<NotifyUserQuick>> Subscribers()
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(user.Subscribers.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpPost("change_subscription/{id:guid}")]
    public async Task<IActionResult> ChangeSubscription(Guid id)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        if (user.Id.Equals(id))
        {
            return BadRequest();
        }
        var toUser = await _users.GetUserAsync(id);
        if (toUser == null)
        {
            return BadRequest();
        }

        if (user.Subscribers.Contains(toUser) || toUser.Subscriptions.Contains(user))
        {
            toUser.Subscriptions.Remove(user);
            user.Subscribers.Remove(toUser);
        }
        else
        {
            toUser.Subscriptions.Add(user);
            user.Subscribers.Add(toUser);
        }

        await _users.UpdateUserAsync(toUser);
        await _users.UpdateUserAsync(user);

        return NoContent();
    }
}
