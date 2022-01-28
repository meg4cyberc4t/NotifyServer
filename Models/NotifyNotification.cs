using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public enum RepeatMode
{
    None,
    Everyday,
    Everyweek,
    Everymonth,
    Everyyear,
}

public record NotifyNotificationInput(
    [StringLength(50)] [Required] string Title,
    string Description,
    [Required] RepeatMode RepeatMode,
    [Required] DateTime Deadline,
    [Required] bool Important
);

public record NotifyNotificationDetailed(
    Guid Id,
    [StringLength(50)] [Required] string Title,
    string Description,
    bool Important,
    RepeatMode RepeatMode,
    DateTime Deadline,
    int ParticipantsCount,
    NotifyUserQuick Creator,
    int UniqueClaim
);

public class NotifyNotification
{
    public Guid Id { get; set; }
    [StringLength(50)] [Required] public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; set; } = null!;
    public bool Important;
    public RepeatMode RepeatMode { get; set; } = RepeatMode.None;
    public DateTime Deadline { get; set; }
    public NotifyUser Creator { get; set; } = null!;

    public int UniqueClaim { get; init; }

    public NotifyNotificationDetailed ToNotifyNotificationDetailed()
    {
        return new NotifyNotificationDetailed(
            Id: Id,
            Title: Title,
            Description: Description,
            Important: Important,
            RepeatMode: RepeatMode,
            Deadline: Deadline,
            Creator: Creator.ToNotifyUserQuick(),
            UniqueClaim: UniqueClaim,
            ParticipantsCount: Participants.Count
        );
    }
}
