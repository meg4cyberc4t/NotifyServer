using Microsoft.AspNetCore.Mvc;
using NotifyServer.Models;

namespace NotifyServer.Controllers;

public class AuthController : Controller
{
    public AuthController(AppDbContext context)
    {
        // _context = context;
        _userRepository = new NotifyUserRepository(context);
    }
    // private readonly AppDbContext  _context;
    private readonly INotifyUserRepository _userRepository;

    [HttpGet("{uid}", Name = "GetById")]
    public async Task<ActionResult<NotifyUser>> Get(string uid)
    {
        var item = await _userRepository.Get(uid);
        return item == null ? NotFound() : item;
    }
}
