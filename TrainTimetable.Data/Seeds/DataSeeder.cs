using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;

namespace TrainTimetable.Data.Seeds;

internal static class DataSeeder
{
    internal static async Task SeedDevelopmentDataAsync(DbContext _dbContext)
    {
        var context = _dbContext as AppDbContext;

        if (context == null)
            throw new ArgumentNullException("Argument dbContext is invalid. The context must be not null.");

        // Country
        if (!await context.Countries.AnyAsync())
        {
            await context.Countries.AddRangeAsync(new List<Country>
            {
                new() { Name = "Croatia" }
            });

            await context.SaveChangesAsync();
        }

        var croatia = await context.Countries.FirstAsync(c => c.Name == "Croatia");

        // Station
        if (!await context.Stations.AnyAsync())
        {
            await context.Stations.AddRangeAsync(new List<Station>
            {
                new() { Name = "Varaždin", BaseStationID = null, Country = croatia },
                new() { Name = "Turčin", BaseStationID = 1, Country = croatia },
                new() { Name = "Doljan", BaseStationID = 1, Country = croatia },
                new() { Name = "Krušljevec", BaseStationID = 1, Country = croatia },
                new() { Name = "Čakovec", BaseStationID = 1, Country = croatia }
            });

            await context.SaveChangesAsync();
        }

        var stations = await context.Set<Station>().ToListAsync();

        // TrainManufacturer
        if (!await context.TrainManufacturers.AnyAsync())
        {
            await context.TrainManufacturers.AddRangeAsync(new List<TrainManufacturer>
            {
                new() { Name = "FS Trenitalia" },
                new() { Name = "Bombardier Transportation" },
                new() { Name = "Alstom" }
            });

            await context.SaveChangesAsync();
        }

        var trainManufacturers = await context.Set<TrainManufacturer>().ToListAsync();

        // Train
        if (!await context.Trains.AnyAsync())
        {
            await context.Trains.AddRangeAsync(new List<Train>
            {
                new() { TrainManufacturer = trainManufacturers[0], Name = "E.403", SeatCount = 60 },
                new() { TrainManufacturer = trainManufacturers[1], Name = "S Stock", SeatCount = 50 },
                new() { TrainManufacturer = trainManufacturers[2], Name = "X65", SeatCount = 76 }
            });

            await context.SaveChangesAsync();
        }

        var trains = await context.Set<Train>().ToListAsync();

        // Line
        if(!await context.Lines.AnyAsync())
        {
            await context.Lines.AddRangeAsync(new List<Line>
            {
                new() { LineNumber = 1 },
                new() { LineNumber = 2 }
            });

            await context.SaveChangesAsync();
        }

        var lines = await context.Set<Line>().ToListAsync();

        // LineSchedule
        if (!await context.LineSchedules.AnyAsync())
        {
            await context.LineSchedules.AddRangeAsync(new List<LineSchedule>
            {
                new() { Line = lines[0], Train = trains[0], StartTime = new TimeOnly(8, 30, 0), DriveDays = DrivingDays.Any },
                new() { Line = lines[0], Train = trains[1], StartTime = new TimeOnly(10, 10, 0), DriveDays = DrivingDays.WorkDays },

                new() { Line = lines[1], Train = trains[0], StartTime = new TimeOnly(9, 30, 0), DriveDays = DrivingDays.WorkDays },
                new() { Line = lines[1], Train = trains[1], StartTime = new TimeOnly(11, 10, 0), DriveDays = DrivingDays.Any }
            });

            await context.SaveChangesAsync();
        }

        // Stop
        if (!await context.Stops.AnyAsync())
        {
            await context.Stops.AddRangeAsync(new List<Stop>
            {
                new() { Station = stations[3], Line = lines[0], Order = 1, ArrivalOffset = null, DepartureOffset = new TimeSpan(0, 5, 0) },
                new() { Station = stations[2], Line = lines[0], Order = 2, ArrivalOffset = new TimeSpan(0, 20, 0), DepartureOffset = new TimeSpan(0, 25, 0) },
                new() { Station = stations[1], Line = lines[0], Order = 3, ArrivalOffset = new TimeSpan(0, 40, 0), DepartureOffset = new TimeSpan(0, 45, 0) },
                new() { Station = stations[0], Line = lines[0], Order = 4, ArrivalOffset = new TimeSpan(0, 55, 0), DepartureOffset = null },

                new() { Station = stations[1], Line = lines[1], Order = 1, ArrivalOffset = null, DepartureOffset = new TimeSpan(0, 5, 0) },
                new() { Station = stations[0], Line = lines[1], Order = 2, ArrivalOffset = new TimeSpan(0, 30, 0), DepartureOffset = new TimeSpan(0, 40, 0) },
                new() { Station = stations[4], Line = lines[1], Order = 3, ArrivalOffset = new TimeSpan(0, 50, 0), DepartureOffset = null }
            });

            await context.SaveChangesAsync();
        }
    }
}