using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace api.Context
{
    public class ParkUpDbContextFactory : IDesignTimeDbContextFactory<ParkUpDbContext>
    {
        public ParkUpDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParkUpDbContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Username=design_time;Password=design_time;Database=design_time");

            return new ParkUpDbContext(optionsBuilder.Options);
        }
    }
}