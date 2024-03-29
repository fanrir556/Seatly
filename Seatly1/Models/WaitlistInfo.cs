using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class WaitlistInfo
{
    public int WaitlistId { get; set; }

    public int? RestaurantId { get; set; }

    public int? SmallTableCurrentNumber { get; set; }

    public int? MediumTableCurrentNumber { get; set; }

    public int? LargeTableCurrentNumber { get; set; }

    public int? SmallTableLastWaitingNumber { get; set; }

    public int? MediumTableLastWaitingNumber { get; set; }

    public int? LargeTableLastWaitingNumber { get; set; }
}
