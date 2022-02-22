using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public record NotifyFolderInput(
    [StringLength(50)] [Required] string Title,
    string Description
);

public record NotifyFolderDetailed(
    Guid Id,
    string Title,
    string Description,
    int ParticipantsCount,
    int NotificationsCount
);

public class NotifyFolder
{
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; init; } = null!;
    public ICollection<NotifyNotification> NotificationsList { get; init; } = null!;
    public NotifyUser Creator { get; init; } = null!;

    public NotifyFolderDetailed ToNotifyFolderDetailed()
    {
        return new NotifyFolderDetailed(Id: Id, Title: Title, Description: Description,
            ParticipantsCount: Participants.Count, NotificationsCount: NotificationsList.Count);
    }
}
