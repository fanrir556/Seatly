using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class BookingOrder
{
    public int OrderId { get; set; }

    public int? WaitingNumber { get; set; }

    public string? WaitingName { get; set; }

    public string? ContactInfo { get; set; }

    public int? RestaurantId { get; set; }

    public DateOnly? Date { get; set; }

    public int? PartySize { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<NotificationRecord> NotificationRecords { get; set; } = new List<NotificationRecord>();
}
