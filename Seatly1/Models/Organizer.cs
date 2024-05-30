using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Seatly1.Models;

public partial class Organizer
{
    [DisplayName("主辦方ID")]
    public int OrganizerId { get; set; }
    [DisplayName("主辦方帳號")]
    public string? OrganizerAccount { get; set; }
    [DisplayName("登入密碼")]
    public string? LoginPassword { get; set; }
    [DisplayName("主辦方名稱")]
    public string? OrganizerName { get; set; }
    [DisplayName("主辦方分類")]
    public string? OrganizerCategory { get; set; }
    [DisplayName("主辦方照片")]
    public string? OrganizerPhoto { get; set; }
    [DisplayName("菜單")]
    public string? Menu { get; set; }
    [DisplayName("主辦方地址")]
    public string? Address { get; set; }
    [DisplayName("主辦方網頁連接")]
    public string? ReservationUrl { get; set; }
    [DisplayName("主辦方Hashtag")]
    public string? Hashtag { get; set; }
    [DisplayName("Email ")]
    public string? Email { get; set; }
    [DisplayName("聯絡電話")]
    public string? Phone { get; set; }
    [DisplayName("認證")]
    public bool? Validation { get; set; }
}
