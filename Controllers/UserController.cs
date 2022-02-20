using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        return user.ToNotifyUserDetailed();
    }

    [HttpPost]
    public async Task<ActionResult<NotifyUserDetailed>> Create([FromBody] NotifyUserInput input)
    {
        var uid = HttpContext.User.Claims.First(e => e.Type == "user_id").Value;
        var user = await _users.GetUserByForgeinUidAsync(uid);
        if (user != null) return Conflict();
        var newUser = new NotifyUser
        {
            Id = Guid.NewGuid(),
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Color = input.Color,
            Subscribers = new List<NotifyUser>(),
            Subscriptions = new List<NotifyUser>(),
            ForgeinUid = uid
        };
        await _users.CreateUserAsync(newUser);
        return Ok(newUser.ToNotifyUserDetailed());
    }

    [HttpPut]
    public async Task<ActionResult<NotifyUserQuick>> Put([FromBody] NotifyUserInput updatedUser)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        user = await _users.GetUserAsync(user.Id);
        if (user == null)
        {
            return BadRequest();
        }
        user.Color = updatedUser.Color;
        user.Firstname = updatedUser.Firstname;
        user.Lastname = updatedUser.Lastname;
        await _users.UpdateUserAsync(user);
        return Ok(user.ToNotifyUserDetailed());
    }

    [HttpGet("/subscriptions")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> Subscriptions()
    {
        var uid = HttpContext.User.Claims.ToList()[4].Value;
        var user = (await _users.GetUserByForgeinUidAsync(uid))!;
        return Ok(user.Subscriptions.Select(e => e.ToNotifyUserQuick()));
    }

    [HttpGet("/subscribers")]
    public async Task<ActionResult<IEnumerable<NotifyUserQuick>>> Subscribers()
    {
        var uid = HttpContext.User.Claims.ToList()[4].Value;
        var user = (await _users.GetUserByForgeinUidAsync(uid))!;
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

        var fromUser = await _users.GetUserAsync(user.Id);
        var toUser = await _users.GetUserAsync(id);
        if (fromUser == null || toUser == null)
        {
            return BadRequest();
        }

        if (fromUser.Subscribers.Contains(toUser) || toUser.Subscriptions.Contains(fromUser))
        {
            toUser.Subscriptions.Remove(fromUser);
            fromUser.Subscribers.Remove(toUser);
        }
        else
        {
            toUser.Subscriptions.Add(fromUser);
            fromUser.Subscribers.Add(toUser);
        }

        await _users.UpdateUserAsync(toUser);
        await _users.UpdateUserAsync(fromUser);

        return NoContent();
    }
}
