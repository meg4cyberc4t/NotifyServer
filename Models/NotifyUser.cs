using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotifyServer.Models;

public record NotifyUserInput(
    [StringLength(50)] [Required] string Firstname,
    [StringLength(50)] [Required] string Lastname,
    [Required] Int64 Color
);

public record NotifyUserQuick(
    Guid Id,
    string Firstname,
    string Lastname,
    Int64 Color
);

public record NotifyUserDetailed(
    Guid Id,
    string Firstname,
    string Lastname,
    Int64 Color,
    int SubscriptionsCount,
    int SubscribersCount
);

public class NotifyUser
{
    public Guid Id { get; init; }
    [StringLength(50)] [Required] public string Firstname { get; set; } = null!;
    [StringLength(50)] [Required] public string Lastname { get; set; } = null!;
    public Int64 Color { get; set; }
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

    public NotifyUserDetailed ToNotifyUserDetailed()
    {
        return new NotifyUserDetailed(
            Id: Id,
            Firstname: Firstname,
            Lastname: Lastname,
            Color: Color,
            SubscriptionsCount: Subscriptions.Select(e => e.ToNotifyUserQuick()).Count(),
            SubscribersCount: Subscribers.Select(e => e.ToNotifyUserQuick()).Count()
        );
    }
}
