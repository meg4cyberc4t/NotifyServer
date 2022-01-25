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
        Console.WriteLine(!(context.Request.Path.StartsWithSegments("/users") && context.Request.Method == "POST"));
        try
        {
            var uid = context.User.Claims.ToList()[4].Value;
            var user = await _db.Users.FirstAsync(e => e.ForgeinUid == uid);
            context.Items.Add("User", user);
        }
        catch (Exception)
        {
            context.Response.StatusCode = 403;
            return;
        }

        await next(context);
    }
}
