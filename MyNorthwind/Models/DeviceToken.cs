using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNorthwind.Models;

public partial class DeviceToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeviceTokenId { get; set; }

    // FCM device token to send push notifications
    public string Token { get; set; }

    // The date when the token was registered
    public DateTime RegisteredAt { get; set; }
}