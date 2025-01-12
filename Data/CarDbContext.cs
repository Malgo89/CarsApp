using Microsoft.EntityFrameworkCore;

namespace CarsApp.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarsApp.Models.Car> Cars { get; set; }
    }
}
