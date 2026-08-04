using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public class ParkUpDbContext : DbContext
    {
        public ParkUpDbContext(DbContextOptions<ParkUpDbContext> options) : base(options) {}
        public DbSet<Building> Buildings { get; set; } = null!;
        public DbSet<EligibilityType> EligibilityTypes { get; set; } = null!;
        public DbSet<ParkingReservation> ParkingReservations { get; set; } = null!;
        public DbSet<ParkingSpot> ParkingSpots { get; set; } = null!;
        public DbSet<Requester> Requesters { get; set; } = null!;
        public DbSet<RequesterEligibility> RequesterEligibilities { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingSpot>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ParkingReservation>()
                .HasOne(r => r.ParkingSpot)
                .WithMany(s => s.ParkingReservations)
                .HasForeignKey(r => r.ParkingSpotId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParkingReservation>()
                .HasOne(r => r.Requester)
                .WithMany(req => req.ParkingReservations)
                .HasForeignKey(r => r.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParkingReservation>()
                .HasOne(r => r.Status)
                .WithMany(status => status.ParkingReservations)
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Requester>()
                .HasIndex(r => r.Email)
                .IsUnique();

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "Accepted" },
                new Status { Id = 3, Name = "Rejected" },
                new Status { Id = 4, Name = "Cancelled" }
            );

            modelBuilder.Entity<EligibilityType>().HasData(
                new EligibilityType { Id = 1, Name = "Disabled" },
                new EligibilityType { Id = 2, Name = "LargeFamily" },
                new EligibilityType { Id = 3, Name = "OversizedVehicle" }
            );
        }
    }
}
