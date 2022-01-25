using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotifyServer.Models;

public class NotifyFolder
{
    public Guid Id { get; set; }
    [StringLength(50)] [Required] public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; set; } = null!;
    public ICollection<NotifyNotification> NotificationsList { get; set; } = null!;
    public NotifyUser Creator { get; set; } = null!;
}
