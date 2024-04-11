namespace Seatly1.DTO
{
    public class OrganizerLoginDTO
    {
        public int OrganizerID { get; set; }
        public string? OrganizerAccount { get; set; }

        public string? LoginPassword { get; set; }

        public bool? Validation { get; set; }

    }
}
