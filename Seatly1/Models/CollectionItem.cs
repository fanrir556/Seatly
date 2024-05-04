using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class CollectionItem
{
    public int SerialId { get; set; }

    public string? UserId { get; set; }

    public int? ActivityId { get; set; }
}
