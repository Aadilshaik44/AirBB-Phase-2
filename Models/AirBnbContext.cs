using Microsoft.EntityFrameworkCore;

namespace AirBB.Models;

public class AirBnbContext(DbContextOptions<AirBnbContext> options) : DbContext(options)
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Residence> Residences => Set<Residence>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // seed Locations
        mb.Entity<Location>().HasData(
            new Location { LocationId = 1, Name = "Chicago" },
            new Location { LocationId = 2, Name = "New York" },
            new Location { LocationId = 3, Name = "Boston" },
            new Location { LocationId = 4, Name = "Miami" }
        );

        // seed Residences
        mb.Entity<Residence>().HasData(
            new Residence { ResidenceId = 101, Name = "Chicago Loop Apartment", ResidencePicture = "chi_loop.jpg", LocationId = 1, GuestNumber = 4, BedroomNumber = 2, BathroomNumber = 1, PricePerNight = 189 },
            new Residence { ResidenceId = 102, Name = "Lincoln Park Flat", ResidencePicture = "chi_lincoln.jpg", LocationId = 1, GuestNumber = 3, BedroomNumber = 1, BathroomNumber = 1, PricePerNight = 139 },
            new Residence { ResidenceId = 201, Name = "NYC Soho Loft", ResidencePicture = "nyc_soho.jpg", LocationId = 2, GuestNumber = 2, BedroomNumber = 1, BathroomNumber = 1, PricePerNight = 259 },
            new Residence { ResidenceId = 301, Name = "Boston Back Bay Condo", ResidencePicture = "bos_backbay.jpg", LocationId = 3, GuestNumber = 4, BedroomNumber = 2, BathroomNumber = 2, PricePerNight = 209 },
            new Residence { ResidenceId = 401, Name = "Miami Beach House", ResidencePicture = "mia_beach.jpg", LocationId = 4, GuestNumber = 6, BedroomNumber = 3, BathroomNumber = 2, PricePerNight = 299 }
        );

        mb.Entity<Reservation>().HasData(
            new Reservation { ReservationId = 1, ResidenceId = 101, ReservationStartDate = new DateTime(2026, 01, 01), ReservationEndDate = new DateTime(2026, 01, 03) }
        );
    }
}
