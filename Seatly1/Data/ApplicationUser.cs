using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Seatly1.Data
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(20)]
        public string? MemberAccount { get; set; }

        [MaxLength(10)]
        public string? MemberNickname { get; set; }


        public int? Permission { get; set; }


        public int? Points { get; set; }

        public int? Age { get; set; }

        [MaxLength(1)]
        public string? Sex { get; set; }

        public String? Birthday { get; set; }
        public DateTime? CreatAt { get; set; }

    }
}
