using System.ComponentModel.DataAnnotations;

namespace Seatly1.Models
{
    public partial class Friends
    {
        [Key]
        public int FlowID { get; set; }

        public int? UserID { get; set; }

        [Display(Name = "使用者帳號")]
        public string? UserName { get; set; }

        [Display(Name = "好友帳號")]
        public string? FriendUserName { get; set; }
    }
}
