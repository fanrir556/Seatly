using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Seatly1.Models;

public partial class AspNetUser
{
    [DisplayName("使用者ID")]
    public string Id { get; set; } = null!;
    [DisplayName("使用者名稱")]
    public string? UserName { get; set; }
    [DisplayName("使用者一般名稱")]
    public string? NormalizedUserName { get; set; }
    [DisplayName("Email")]
    public string? Email { get; set; }
    [DisplayName("NormalizedEmail")]
    public string? NormalizedEmail { get; set; }
    [DisplayName("Email認證")]
    public bool EmailConfirmed { get; set; }
    [DisplayName("密碼")]
    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }
    [DisplayName("電話")]
    public string? PhoneNumber { get; set; }
    [DisplayName("電話認證")]
    public bool PhoneNumberConfirmed { get; set; }
    [DisplayName("雙認證")]
    public bool TwoFactorEnabled { get; set; }
    [DisplayName("鎖定狀態")]
    public DateTimeOffset? LockoutEnd { get; set; }
    [DisplayName("鎖定狀態")]
    public bool LockoutEnabled { get; set; }
    [DisplayName("登入錯誤次數")]
    public int AccessFailedCount { get; set; }
    [DisplayName("年齡")]
    public int? Age { get; set; }
    [DisplayName("生日")]
    public DateTime? Birthday { get; set; }
    [DisplayName("創建日期")]
    public DateTime? CreatAt { get; set; }
    [DisplayName("使用者真實姓名")]
    public string? MemberRealName { get; set; }
    [DisplayName("使用者暱稱")]
    public string? MemberNickname { get; set; }
    [DisplayName("權限")]
    public int? Permission { get; set; }
    [DisplayName("累積點數")]
    public int? Points { get; set; }
    [DisplayName("性別")]
    public string? Sex { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; } = new List<AspNetUserRole>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();
}
