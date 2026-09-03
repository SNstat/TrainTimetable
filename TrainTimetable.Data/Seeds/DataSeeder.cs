using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;

namespace TrainTimetable.Data.Seeds;

internal static class DataSeeder
{
    internal static void SeedDevelopmentData(DbContext _dbContext)
    {
        var context = _dbContext as AppDbContext;

        // Country
        if (!context.Countries.Any())
        {
            var countries = new List<Country>()
            {
                new() { Name = "Croatia" }
            };

            context.Countries.AddRange(countries);
        }

        // Station
        var croatia = context.Countries.First(c => c.Name == "Croatia");
        if (!context.Stations.Any(_ => _.Country == croatia))
        {
            context.Stations.AddRange(new List<Station>()
            {
                new() { Name = "Varaždin", BaseStationID = null, Country = croatia },
                new() { Name = "Turčin", BaseStationID = 1, Country = croatia },
                new() { Name = "Doljan", BaseStationID = 1, Country = croatia },
                new() { Name = "Krušljevec", BaseStationID = 1, Country = croatia },
                new() { Name = "Čakovec", BaseStationID = 1, Country = croatia }
            });
        }
        var stations = context.Stations.Where(_ => _.Country == croatia).ToList();

        return;
        // TODO : za Šimuna ;)
       
        // TrainManufacturer
        var trainManufacturers = new List<TrainManufacturer>()
        {
            new() { Name = "FS Trenitalia" },
            new() { Name = "Bombardier Transportation" },
            new() { Name = "Alstom" }
        };

        context.Set<TrainManufacturer>().AddRange(trainManufacturers);

        // Train
        var trains = new List<Train>()
        {
            new() { TrainManufacturer = trainManufacturers[0], Name = "E.403", SeatCount = 60 },
            new() { TrainManufacturer = trainManufacturers[1], Name = "S Stock", SeatCount = 50 },
            new() { TrainManufacturer = trainManufacturers[2], Name = "X65", SeatCount = 76 }
        };

        context.Set<Train>().AddRange(trains);

        // Line
        var lines = new List<Line>()
        {
            new() { LineNumber = 1 },
            new() { LineNumber = 2 }
        };

        context.Set<Line>().AddRange(lines);

        // LineSchedule
        var lineSchedules = new List<LineSchedule>()
        {
            new() { Line = lines[0], Train = trains[0], StartTime = new TimeOnly(8, 30, 0), DriveDays = DrivingDays.Any },
            new() { Line = lines[0], Train = trains[1], StartTime = new TimeOnly(10, 10, 0), DriveDays = DrivingDays.WorkDays },

            new() { Line = lines[1], Train = trains[0], StartTime = new TimeOnly(9, 30, 0), DriveDays = DrivingDays.WorkDays },
            new() { Line = lines[1], Train = trains[1], StartTime = new TimeOnly(11, 10, 0), DriveDays = DrivingDays.Any }
        };

        context.Set<LineSchedule>().AddRange(lineSchedules);

        // Stop
        var stops = new List<Stop>()
        {
            new() { Station = stations[3], Line = lines[0], Order = 1, ArrivalOffset = null, DepartureOffset = new TimeSpan(0, 5, 0) },
            new() { Station = stations[2], Line = lines[0], Order = 2, ArrivalOffset = new TimeSpan(0, 20, 0), DepartureOffset = new TimeSpan(0, 25, 0) },
            new() { Station = stations[1], Line = lines[0], Order = 3, ArrivalOffset = new TimeSpan(0, 40, 0), DepartureOffset = new TimeSpan(0, 45, 0) },
            new() { Station = stations[0], Line = lines[0], Order = 4, ArrivalOffset = new TimeSpan(0, 55, 0), DepartureOffset = null },

            new() { Station = stations[1], Line = lines[1], Order = 1, ArrivalOffset = null, DepartureOffset = new TimeSpan(0, 5, 0) },
            new() { Station = stations[0], Line = lines[1], Order = 2, ArrivalOffset = new TimeSpan(0, 30, 0), DepartureOffset = new TimeSpan(0, 40, 0) },
            new() { Station = stations[4], Line = lines[1], Order = 3, ArrivalOffset = new TimeSpan(0, 50, 0), DepartureOffset = null }
        };

        _dbContext.Set<Stop>().AddRange(stops);

        _dbContext.SaveChanges();
    }
}