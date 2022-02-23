using System.Security;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;

namespace NotifyServer.Middleware;

public class FactoryCheckCreatingUserMiddleware : IMiddleware
{
    private readonly AppDbContext _db;

    public FactoryCheckCreatingUserMiddleware(AppDbContext db)
    {
        _db = db;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var uid = context.User.Claims.First(e => e.Type == "user_id").Value;
        var user = await _db.Users.FirstOrDefaultAsync(e => e.ForgeinUid == uid);
        if (user == null)
        {
            // There is no entry in db
            context.Response.StatusCode = 403;
            return;
        }
        context.Items.Add("User", user);
        await next(context);
    }
}
