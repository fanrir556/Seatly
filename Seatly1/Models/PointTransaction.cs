using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class PointTransaction
{
    public int Id { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public bool? Active { get; set; }
}
