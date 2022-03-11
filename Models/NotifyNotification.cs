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

public record NotifyNotificationQuick(
    Guid Id,
    string Title,
    string Description,
    bool Important,
    RepeatMode RepeatMode,
    DateTime Deadline,
    NotifyUserQuick Creator,
    int UniqueClaim
);

public record NotifyNotificationDetailed(
    Guid Id,
    string Title,
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
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<NotifyUser> Participants { get; init; } = null!;

    public bool Important { get; set; }
    public RepeatMode RepeatMode { get; set; } = RepeatMode.None;
    public DateTime Deadline { get; set; }
    public NotifyUser Creator { get; init; } = null!;

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

    public NotifyNotificationQuick ToNotifyNotificationQuick()
    {
        return new NotifyNotificationQuick(
            Id: Id,
            Title: Title,
            Description: Description,
            Important: Important,
            RepeatMode: RepeatMode,
            Deadline: Deadline,
            Creator: Creator.ToNotifyUserQuick(),
            UniqueClaim: UniqueClaim
        );
    }
}
