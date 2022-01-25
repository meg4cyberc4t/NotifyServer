using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotifyServer.Models;

public enum RepeatMode
{
    None,
    Everyday,
    Everyweek,
    Everymonth,
    Everyyear,
}

public class NotifyNotification
{
    public Guid Id { get; set; }
    [StringLength(50)] [Required] public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; set; } = null!;
    public RepeatMode RepeatMode { get; set; } = RepeatMode.None;
    public DateTime Deadline { get; set; }
    public NotifyUser Creator { get; set; } = null!;
}
