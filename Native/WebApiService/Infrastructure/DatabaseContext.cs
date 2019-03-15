using Microsoft.EntityFrameworkCore;
using WebApiService.Models;

namespace WebApiService.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        

        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
    }
}
