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
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
