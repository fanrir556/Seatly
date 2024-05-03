using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class DailyCheckIn
{
    public int Id { get; set; }

    public string? MemberId { get; set; }

    public DateOnly? CheckInTime { get; set; }
}
