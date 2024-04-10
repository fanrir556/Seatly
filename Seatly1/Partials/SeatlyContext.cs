using Microsoft.EntityFrameworkCore;

namespace Seatly1.Models
{
    public partial class SeatlyContext : DbContext
    {
        public object Collections { get; internal set; }

        // 定義OnConfiguring函式

        //定義OnConfiguring函式
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration Config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(Config.GetConnectionString("Seatly"));
            }
        }

    }
}
