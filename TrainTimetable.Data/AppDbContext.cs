using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data;

public class AppDbContext : DbContext
{
    public DbSet<Train> Trains { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Stop> Stops { get; set; }

    public AppDbContext()
    {

    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrainTimetable;Integrated Security=True");
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Train>(_ => {
            _.Property(x => x.Name).HasMaxLength(100);
            _.Property(x => x.Manufacturer).HasMaxLength(100);
            _.HasData(
                new Train { ID = 1, Manufacturer = "FS Trenitalia", Name = "E.403", SeatCount = 60 },
                new Train { ID = 2, Manufacturer = "Bombardier Transportation", Name = "S Stock", SeatCount = 50 },
                new Train { ID = 3, Manufacturer = "Alstom", Name = "SL X60", SeatCount = 76 });
        });

        modelBuilder.Entity<Country>(_ =>
        {
            _.Property(x => x.Name).HasMaxLength(100);
            _.HasData(
                new Country { ID = 1, Name = "Croatia"}
                );
        });

        modelBuilder.Entity<Station>()
            .HasIndex(_ => _.Name)
            .IsUnique();

        modelBuilder.Entity<Station>(_ => {
            _.Property(x => x.Name).HasMaxLength(100);
            _.HasData(
                new Station { ID = 1, Name = "Varaždin", BaseStationID = null, CountryID = 1 },
                new Station { ID = 2, Name = "Turčin", BaseStationID = 1, CountryID = 1 },
                new Station { ID = 3, Name = "Doljan", BaseStationID = 1, CountryID = 1 },
                new Station { ID = 4, Name = "Krušljevec", BaseStationID = 1, CountryID = 1 },
                new Station { ID = 5, Name = "Čakovec", BaseStationID = 1, CountryID = 1 });
        });

        modelBuilder.Entity<Line>(_ =>
        {
            _.HasData(
                new Line { ID = 1, LineNumber = 1 },
                new Line { ID = 2, LineNumber = 2 }
                );
        });

        modelBuilder.Entity<Stop>(_ =>
        {
            _.HasData(
                new Stop { ID = 1, ArrivalTime = null, DepartureTime = new TimeOnly(10, 35, 0), StationID = 4, LineID = 1, Order = 1 },
                new Stop { ID = 2, ArrivalTime = new TimeOnly(10, 50, 0), DepartureTime = new TimeOnly(10, 55, 0), StationID = 3, LineID = 1, Order = 2 },
                new Stop { ID = 3, ArrivalTime = new TimeOnly(11, 10, 0), DepartureTime = new TimeOnly(11, 15, 0), StationID = 2, LineID = 1, Order = 3 },
                new Stop { ID = 4, ArrivalTime = new TimeOnly(11, 30, 0), DepartureTime = null, StationID = 1, LineID = 1, Order = 4 },

                new Stop { ID = 5, ArrivalTime = null, DepartureTime = new TimeOnly(12, 35, 0), StationID = 3, LineID = 2, Order = 1 },
                new Stop { ID = 6, ArrivalTime = new TimeOnly(12, 50, 0), DepartureTime = new TimeOnly(12, 55, 0), StationID = 1, LineID = 2, Order = 2 },
                new Stop { ID = 7, ArrivalTime = new TimeOnly(13, 10, 0), DepartureTime = null, StationID = 5, LineID = 2, Order = 3 }
                );
        });

        base.OnModelCreating(modelBuilder);
    }
}
