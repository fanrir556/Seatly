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

        public List<string>? SList1 { get; set; }

        public List<string>? SList2 { get; set; }
        public List<string>? DNames { get; set; }
        public bool isMg { get; set; } = false;
    }
}
