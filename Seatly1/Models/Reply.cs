﻿using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class Reply
{
    public int ReplyId { get; set; }

    public int? ReviewId { get; set; }

    public string? ReplyAccount { get; set; }

    public string? ReContent { get; set; }
}
