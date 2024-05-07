using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? MemberAccount { get; set; }

    public string? RestaurantAccount { get; set; }

    public string? ReContent { get; set; }
}
