using System.ComponentModel.DataAnnotations;

namespace Seatly1.Models
{
    public partial class Organizers
    {
        [Key]
        public int OrganizerID { get; set; }

        [Display(Name = "Organizer Account")]
        public string? OrganizerAccount { get; set; }

        [Display(Name = "Login Password")]
        public string? LoginPassword { get; set; }

        [Display(Name = "Organizer Name")]
        public string? OrganizerName { get; set; }

        [Display(Name = "Organizer Category")]
        public string? OrganizerCategory { get; set; }

        [Display(Name = "Organizer Photo")]
        public string? OrganizerPhoto { get; set; }

        public string? Menu { get; set; }

        public string? Address { get; set; }

        [Display(Name = "Reservation URL")]
        public string? ReservationURL { get; set; }

        public string? Hashtag { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public bool? Validation { get; set; }
    }
}
