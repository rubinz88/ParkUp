using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace api.Context
{
    public class ParkUpDbContextFactory : IDesignTimeDbContextFactory<ParkUpDbContext>
    {
        public ParkUpDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParkUpDbContext>();

            // Ez a connection string CSAK a "dotnet ef migrations" parancsokhoz kell,
            // magat az adatbazist nem eri el (csak a semat generalja a modellbol).
            // Futasidoben (docker-compose-ban) tovabbra is a Program.cs-ben
            // osszeallitott, kornyezeti valtozokbol epulo connection string szamit.
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Username=design_time;Password=design_time;Database=design_time");

            return new ParkUpDbContext(optionsBuilder.Options);
        }
    }
}