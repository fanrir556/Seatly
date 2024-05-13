using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class PointStore
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? Category { get; set; }

    public int? ProductPrice { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductImage { get; set; }
}
