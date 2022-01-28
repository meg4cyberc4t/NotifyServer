using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public record NotifyFolderInput(
    [StringLength(50)] [Required] string Title,
    string Description
);

public record NotifyFolderDetailed(
    Guid Id,
    [StringLength(50)] [Required] string Title,
    string Description,
    int ParticipantsCount,
    int NotificationsCount
);

public class NotifyFolder
{
    public Guid Id { get; set; }
    [StringLength(50)] [Required] public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; set; } = null!;
    public ICollection<NotifyNotification> NotificationsList { get; set; } = null!;
    public NotifyUser Creator { get; set; } = null!;

    public NotifyFolderDetailed ToNotifyFolderDetailed()
    {
        return new NotifyFolderDetailed(Id: Id, Title: Title, Description: Description,
            ParticipantsCount: Participants.Count, NotificationsCount: NotificationsList.Count);
    }
}
