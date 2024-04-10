using System;
using System.Collections.Generic;

namespace Seatly1.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public int? Age { get; set; }

    public DateTime? Birthday { get; set; }

    public DateTime? CreatAt { get; set; }

    public string? MemberRealName { get; set; }

    public string? MemberNickname { get; set; }

    public int? Permission { get; set; }

    public int? Points { get; set; }

    public string? Sex { get; set; }
}
