namespace Seatly1.DTO
{
    public class NotificationRecordDTO2
    {
        public int ActivityId { get; set; }

        public int? OrganizerId { get; set; }

        public IFormFile? ActivityPhoto { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Capacity { get; set; }

        public string? ActivityName { get; set; }
        public string? ActivityMethod { get; set; }

        public string? DescriptionN { get; set; }

        public bool? IsRecurring { get; set; }

        public string? RecurringTime { get; set; }
    }
}
