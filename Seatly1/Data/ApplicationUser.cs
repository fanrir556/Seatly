using Microsoft.AspNetCore.Identity;
using Seatly1.Models;
using System.ComponentModel.DataAnnotations;

namespace Seatly1.Data
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(20)]
        public string? MemberRealName { get; set; }

        [MaxLength(10)]
        public string? MemberNickname { get; set; }


        public int? Permission { get; set; }


        public int? Points { get; set; }

        public int? Age { get; set; }

        [MaxLength(1)]
        public string? Sex { get; set; }

        public DateTime? Birthday { get; set; }
        public DateTime? CreatAt { get; set; }


        // 導航屬性
        public ICollection<CollectionItem> Collections { get; set; }
    }
}
