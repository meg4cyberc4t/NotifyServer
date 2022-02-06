using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotifyServer.Models;
using NotifyServer.Repository;

namespace NotifyServer.Controllers;

[Authorize]
[Route("search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly INotifySearchRepository _search;
    private readonly INotifyUserRepository _users;

    public SearchController(AppDbContext context)
    {
        _users = new NotifyUserReposoitoryPg(context);

        _search = new NotifySearchRepositoryPg(context);
    }

    [HttpGet("from_users", Name = "SearchFromUsers")]
    public ActionResult<IEnumerable<NotifyUserQuick>> SearchFromUsers([FromQuery] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }

        return Ok(_search.FromUsersQuery(pattern, limit: limit, offset: offset).Select(e => e.ToNotifyUserQuick()));
    }

    [HttpGet("from_notifications", Name = "SearchFromNotifications")]
    public async Task<ActionResult<IEnumerable<NotifyNotification>>> SearchFromNotifications([FromQuery] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }
        var uid = HttpContext.User.Claims.ToList()[4].Value;
        var user = (await _users.GetUserByForgeinUidAsync(uid))!;
        return Ok(_search.FromNotificationsQuery(pattern, limit: limit, offset: offset).Include(e => e.Participants).Where(e => e.Creator == user || e.Participants.Contains(user)).Select(e => e.ToNotifyNotificationQuick()));
    }

    [HttpGet("from_folders", Name = "SearchFromFolders")]
    public async Task<ActionResult<IEnumerable<NotifyFolderDetailed>>> SearchFromFolders([FromQuery] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }
        var uid = HttpContext.User.Claims.ToList()[4].Value;
        var user = (await _users.GetUserByForgeinUidAsync(uid))!;
        return Ok(_search.FromFoldersQuery(pattern, limit: limit, offset: offset).Include(e => e.Participants).Where(e => e.Creator == user || e.Participants.Contains(user)).Select(e => e.ToNotifyFolderDetailed()));
    }
}
