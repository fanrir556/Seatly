using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Seatly1.Models;

public partial class NotificationRecord
{
    [DisplayName("活動ID")]
    public int ActivityId { get; set; }
    [DisplayName("主辦方ID")]
    public int? OrganizerId { get; set; }
    [DisplayName("活動照片")]
    public byte[]? ActivityPhoto { get; set; }
    [DisplayName("開始時間")]
    public DateTime? StartTime { get; set; }
    [DisplayName("結束時間")]
    public DateTime? EndTime { get; set; }
    [DisplayName("人數上限")]
    public int? Capacity { get; set; }
    [DisplayName("活動名稱")]
    public string? ActivityName { get; set; }
    [DisplayName("活動敘述")]
    public string? DescriptionN { get; set; }
    [DisplayName("使否重複")]
    public bool? IsRecurring { get; set; }
    [DisplayName("重複時間")]
    public string? RecurringTime { get; set; }
    [DisplayName("活動方法")]
    public string? ActivityMethod { get; set; }
    [DisplayName("活動區域")]
    public string? Location { get; set; }
    [DisplayName("活動地點")]
    public string? LocationDescription { get; set; }
    [DisplayName("活動啟動")]
    public bool? IsActivity { get; set; }
    [DisplayName("HashTag1")]
    public string? HashTag1 { get; set; }
    [DisplayName("HashTag2")]
    public string? HashTag2 { get; set; }
    [DisplayName("HashTag3")]
    public string? HashTag3 { get; set; }
    [DisplayName("HashTag4")]
    public string? HashTag4 { get; set; }
    [DisplayName("HashTag5")]
    public string? HashTag5 { get; set; }
}
