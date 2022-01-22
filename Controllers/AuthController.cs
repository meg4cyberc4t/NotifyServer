using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotifyServer.Models;

namespace NotifyServer.Controllers;

[Route("auth/")]
[Authorize]
public class AuthController : Controller
{
    public AuthController(AppDbContext context)
    {
    }


    [HttpGet("/auth/hello", Name = "Test")]
    public ActionResult<string?> Test()
    {
        return "You authorized!";
    }
}
