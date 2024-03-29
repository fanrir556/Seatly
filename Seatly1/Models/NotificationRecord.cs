using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class NotificationRecord
{
    public int NotificationId { get; set; }

    public int? OrderId { get; set; }

    public string? NotificationType { get; set; }

    public string? NotificationContent { get; set; }

    public string? NotificationStatus { get; set; }

    public DateTime? NotificationTimestamp { get; set; }

    public string? EmailAddress { get; set; }

    public bool? MessageSent { get; set; }

    public string? MessageContent { get; set; }

    public virtual BookingOrder? Order { get; set; }
}
