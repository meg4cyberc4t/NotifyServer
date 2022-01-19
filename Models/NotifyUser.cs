using System.ComponentModel.DataAnnotations;

namespace NotifyServer.Models;

public class NotifyUser
{
    [Required]
    [Key]
    public long Id { get; set; }
    [Required]
    public string? UserId { get; set; }
}
