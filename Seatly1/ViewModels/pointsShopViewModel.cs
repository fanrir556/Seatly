using Seatly1.Models;

namespace Seatly1.ViewModels
{
    public class pointsShopViewModel
    {
        public IEnumerable<PointStore> pointsShopPd { get; set; }
        public AspNetUser user { get; set; }
        public IEnumerable<PointTransaction> trans { get; set; }
    }
}
