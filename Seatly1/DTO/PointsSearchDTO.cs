using Seatly1.Models;

namespace Seatly1.DTO
{
    public class PointsSearchDTO
    {
        public string? Cate { get; set; }
        public int? PgNum { get; set; } = 1;
        public int? PgSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortType { get; set; } = "asc";
        public string? Keyword { get; set; }
        public string? SearchBy { get; set; } = "id";
    }
}
