using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public record NotifyUserInput(
    [StringLength(50)] [Required] string Firstname,
    [StringLength(50)] [Required] string Lastname,
    [Required] long Color
);

public record NotifyUserQuick(
    Guid Id,
    string Firstname,
    string Lastname,
    long Color
);

public record NotifyUserDetailed(
    Guid Id,
    string Firstname,
    string Lastname,
    long Color,
    int SubscriptionsCount,
    int SubscribersCount,
    bool Follow
);

public class NotifyUser
{
    public Guid Id { get; init; }
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public long Color { get; set; }
    public ICollection<NotifyUser> Subscriptions { get; init; } = new List<NotifyUser>();
    public ICollection<NotifyUser> Subscribers { get; init; } = new List<NotifyUser>();

    public string ForgeinUid { get; init; } = null!;

    public ICollection<NotifyFolder> Folders { get; set; } = null!;

    public ICollection<NotifyNotification> Notifications { get; set; } = null!;

    public ICollection<NotifyNotification> NotificationsWhereCreator { get; set; } = null!;
    public ICollection<NotifyFolder> FolderWhereCreator { get; set; } = null!;


    public NotifyUserQuick ToNotifyUserQuick()
    {
        return new NotifyUserQuick(
            Id: Id,
            Firstname: Firstname,
            Lastname: Lastname,
            Color: Color
        );
    }

    public NotifyUserDetailed ToNotifyUserDetailed(NotifyUser user)
    {
        return new NotifyUserDetailed(
            Id: Id,
            Firstname: Firstname,
            Lastname: Lastname,
            Color: Color,
            SubscriptionsCount: Subscriptions.Select(e => e.ToNotifyUserQuick()).Count(),
            SubscribersCount: Subscribers.Select(e => e.ToNotifyUserQuick()).Count(),
            Follow: Subscribers.Contains(user)
        );
    }
}
