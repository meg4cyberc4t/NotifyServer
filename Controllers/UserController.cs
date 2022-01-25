using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly NotifyUserRepository _userRepository;

    public UserController(AppDbContext context)
    {
        _userRepository = new NotifyUserRepository(context);
    }

    [HttpGet]
    public IEnumerable<NotifyUserQuick> GetAll()
    {
        Console.WriteLine(HttpContext.Items["User"]);
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
        var uid = HttpContext.User.Claims.ToList()[4].Value;
        return Ok(_userRepository.Create(user, uid).Entity);
    }

    [HttpPut]
    public ActionResult<NotifyUser> Put([FromBody] NotifyUserInput updatedUser)
    {
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        _userRepository.Put(new NotifyUserQuick(
            Id: user.Id,
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
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        _userRepository.ChangeSubscription(user.Id, id);
        return NoContent();
    }
}
