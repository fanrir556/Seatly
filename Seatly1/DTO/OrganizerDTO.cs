namespace Seatly1.DTO
{
    public class OrganizerDTO
    {
        public int OrganizerId { get; set; }

        public string? OrganizerAccount { get; set; }

        public string? LoginPassword { get; set; }

        public string? OrganizerName { get; set; }

        public string? OrganizerPhoto { get; set; }

        public string? ReservationUrl { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool? Validation { get; set; }
    }
}
