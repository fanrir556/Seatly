using Seatly1.Models;

namespace Seatly1.Controllers
{
    internal class SearchViewModel
    {
        public List<NotificationRecord> Activities { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}