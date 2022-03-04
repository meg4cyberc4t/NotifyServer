using System.ComponentModel.DataAnnotations;
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
    public ActionResult<IEnumerable<NotifyUserQuick>> SearchFromUsers([FromQuery][Required] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }

        return Ok(_search.FromUsersQuery(pattern, limit: limit, offset: offset).Select(e => e.ToNotifyUserQuick()));
    }

    [HttpGet("from_notifications", Name = "SearchFromNotifications")]
    public ActionResult<IEnumerable<NotifyNotificationQuick>> SearchFromNotifications([FromQuery][Required] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(_search.FromNotificationsQuery(pattern, limit: limit, offset: offset).Include(e => e.Participants).Where(e => e.Creator == user || e.Participants.Contains(user)).Select(e => e.ToNotifyNotificationQuick()));
    }

    [HttpGet("from_folders", Name = "SearchFromFolders")]
    public ActionResult<IEnumerable<NotifyFolderDetailed>> SearchFromFolders([FromQuery][Required] string pattern, [FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        if (pattern.Length == 0 || offset < 0 || limit > 100)
        {
            return BadRequest();
        }
        var user = (HttpContext.Items["User"] as NotifyUser)!;
        return Ok(_search.FromFoldersQuery(pattern, limit: limit, offset: offset).Include(e => e.Participants).Where(e => e.Creator == user || e.Participants.Contains(user)).Select(e => e.ToNotifyFolderDetailed()));
    }
}
