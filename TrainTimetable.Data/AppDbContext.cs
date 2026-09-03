using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Seeds;

namespace TrainTimetable.Data;

public class AppDbContext : DbContext
{
    public DbSet<TrainManufacturer> TrainManufacturers { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<LineSchedule> LineSchedules { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<TicketSchedule> TicketSchedules { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

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
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrainTimetableDB;Integrated Security=True");
        }

        optionsBuilder.UseSeeding((dbContext, _) =>
            JsonDataSeeder.SeedDevelopmentData(dbContext));

        optionsBuilder.UseAsyncSeeding(async (dbContext, _, _) =>
            await JsonDataSeederAsync.SeedDevelopmentDataAsync(dbContext));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>()
            .Property(_ => _.ID)
            .ValueGeneratedNever();

        modelBuilder.Entity<TrainManufacturer>().HasIndex(_ => _.Name).IsUnique();

        modelBuilder.Entity<Train>(_ =>
        {
            _.HasIndex(_ => _.TrainNumber).IsUnique();
            _.HasIndex(_ => _.Name).IsUnique();
        });

        modelBuilder.Entity<Country>().HasIndex(_ => _.Name).IsUnique();

        modelBuilder.Entity<Station>().HasIndex(_ => _.Name).IsUnique();

        modelBuilder.Entity<Stop>(_ =>
        {
            _.HasIndex(x => new { x.LineID, x.Order }).IsUnique();
            _.HasIndex(x => new { x.LineID, x.StationID }).IsUnique();
        });

        modelBuilder.Entity<TicketSchedule>().HasIndex(_ => new { _.LineScheduleID, _.Date } ).IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
