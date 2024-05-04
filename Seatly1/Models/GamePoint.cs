using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class GamePoint
{
    public int Id { get; set; }

    public int? MemberId { get; set; }

    public DateOnly? PointsDate { get; set; }
}
