using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;

namespace Seatly1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public object Collections { get; internal set; }
        public IEnumerable<object> CollectionItems { get; set; }
        public IEnumerable<object> NotificationRecords { get; internal set; }
    }
}
