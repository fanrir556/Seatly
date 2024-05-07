using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Seatly1.Models;

public partial class CollectionItem
{
    [Key]
    public int SerialId { get; set; }

    public string? UserId { get; set; }

    public int? ActivityId { get; set; }
}
