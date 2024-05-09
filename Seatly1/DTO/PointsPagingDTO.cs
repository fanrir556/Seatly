using Microsoft.AspNetCore.Mvc.Rendering;
using Seatly1.Models;

namespace Seatly1.DTO
{
    public class PointsPagingDTO
    {
        public int TotalPages { get; set; }

        public List<PointStore>? Shops { get; set; }

        public List<PointTransaction>? Trans { get; set; }

        public int? UserPoints { get; set; }

        public SelectList? List1 { get; set; }

        public SelectList? List2 { get; set; }
    }
}
