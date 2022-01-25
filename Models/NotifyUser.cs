using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public record NotifyUserInput(
    string Firstname,
    string Lastname,
    int Color
);

public record NotifyUserQuick(
    Guid Id,
    string Firstname,
    string Lastname,
    int Color
);

public record NotifyUserDetailed(
    Guid Id,
    string Firstname,
    string Lastname,
    int Color,
    int SubscriptionsCount,
    int SubscribersCount
);

public class NotifyUser
{
    public Guid Id { get; set; }
    [StringLength(50)] [Required] public string Firstname { get; set; } = null!;
    [StringLength(50)] [Required] public string Lastname { get; set; } = null!;
    public int Color { get; set; }
    public ICollection<NotifyUser> Subscriptions { get; set; } = new List<NotifyUser>();
    public ICollection<NotifyUser> Subscribers { get; set; } = new List<NotifyUser>();

    public string ForgeinUid { get; set; } = null!;

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
