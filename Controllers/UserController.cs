using System.Formats.Asn1;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

// [Authorize]
[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    readonly NotifyUserRepository _userRepository;

    public UserController(AppDbContext context)
    {
        _userRepository = new NotifyUserRepository(context);
    }


    [HttpGet]
    public IEnumerable<NotifyUserQuick> GetAll()
    {
        return _userRepository.GetAll().Select(e => e.ToNotifyUserQuick());
    }

    [HttpGet("{id:guid}", Name = "GetUserById")]
    public ActionResult<NotifyUserDetailed> Get(Guid id)
    {
        NotifyUser? user = _userRepository.Get(id);
        return user == null
            ? NotFound()
            : Ok(user.ToNotifyUserDetailed());
    }

    [HttpPost]
    public ActionResult<NotifyUser> Create([FromBody] NotifyUserInput user)
    {
        return Ok(_userRepository.Create(user).Entity);
    }

    [HttpPut]
    public ActionResult<NotifyUser> Put([FromBody] NotifyUserInput updatedUser)
    {
        var id = Guid.Parse("b85dc89a-44c4-43e9-91a4-73cd2429f9f6");
        NotifyUser? userRepo = _userRepository.Get(id);
        _userRepository.Put(new NotifyUserQuick(
            Id: id,
            Firstname: updatedUser.Firstname,
            Lastname: updatedUser.Lastname,
            Color: updatedUser.Color
        ));
        return NoContent();
    }

    [HttpGet("{id:guid}/subscriptions")]
    public ActionResult<IEnumerable<NotifyUser>> Subscriptions(Guid id)
    {
        var user = _userRepository.Get(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Subscriptions);
    }

    [HttpGet("{id:guid}/subscribers")]
    public ActionResult<IEnumerable<NotifyUser>> Subscribers(Guid id)
    {
        var user = _userRepository.Get(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Subscribers);
    }
    [HttpPost("change_subscription/{id:guid}")]
    public IActionResult ChangeSubscription(Guid id)
    {
        var user = Guid.Parse("b85dc89a-44c4-43e9-91a4-73cd2429f9f6");
        _userRepository.ChangeSubscription(user, id);
        return NoContent();
    }
}
