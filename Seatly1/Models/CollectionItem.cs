using Seatly1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Seatly1.Models;

public partial class CollectionItem
{
    [Key]
    public int SerialId { get; set; }

    public string? UserId { get; set; }

    public int? ActivityId { get; set; }


    // 導航屬性
    //public ApplicationUser User { get; set; }
    
    //public Activity Activity { get; set; }
}
