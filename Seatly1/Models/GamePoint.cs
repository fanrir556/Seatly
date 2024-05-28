using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class GamePoint
{
    public int Id { get; set; }

    public string? MemberId { get; set; }

    public DateOnly? PointsDate { get; set; }

    public int? GameType { get; set; }
}
