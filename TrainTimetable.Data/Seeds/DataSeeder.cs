using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Seeds;

internal static class DataSeeder
{
    internal static void SeedDevelopmentData(DbContext _dbContext)
    {
        //Country
        var countries = new List<Country>()
        {
            new() { Name = "Croatia" }
        };

        _dbContext.Set<Country>().AddRange(countries);

        //Station
        var stations = new List<Station>()
        {
            new() { Name = "Varaždin", BaseStationID = null, Country = countries[0] },
            new() { Name = "Turčin", BaseStationID = 1, Country = countries[0] },
            new() { Name = "Doljan", BaseStationID = 1, Country = countries[0] },
            new() { Name = "Krušljevec", BaseStationID = 1, Country = countries[0] },
            new() { Name = "Čakovec", BaseStationID = 1, Country = countries[0] }
        };

        _dbContext.Set<Station>().AddRange(stations);

        //TrainManufacturer
        var trainManufacturers = new List<TrainManufacturer>()
        {
            new() { Name = "FS Trenitalia" },
            new() { Name = "Bombardier Transportation" },
            new() { Name = "Alstom" }
        };

        _dbContext.Set<TrainManufacturer>().AddRange(trainManufacturers);

        //Train
        var trains = new List<Train>()
        {
            new() { TrainManufacturer = trainManufacturers[0], Name = "E.403", SeatCount = 60 },
            new() { TrainManufacturer = trainManufacturers[1], Name = "S Stock", SeatCount = 50 },
            new() { TrainManufacturer = trainManufacturers[2], Name = "X65", SeatCount = 76 }
        };

        _dbContext.Set<Train>().AddRange(trains);

        //Line
        var lines = new List<Line>()
        {
            new() { LineNumber = 1, Train = trains[0] },
            new() { LineNumber = 2, Train = trains[1] }
        };

        _dbContext.Set<Line>().AddRange(lines);

        //Stop
        var stops = new List<Stop>()
        {
            new Stop { ArrivalTime = null, DepartureTime = new TimeOnly(10, 35, 0), Station = stations[3], Line = lines[0], Order = 1 },
            new Stop { ArrivalTime = new TimeOnly(10, 50, 0), DepartureTime = new TimeOnly(10, 55, 0), Station = stations[2], Line = lines[0], Order = 2 },
            new Stop { ArrivalTime = new TimeOnly(11, 10, 0), DepartureTime = new TimeOnly(11, 15, 0), Station = stations[1], Line = lines[0], Order = 3 },
            new Stop { ArrivalTime = new TimeOnly(11, 30, 0), DepartureTime = null, Station = stations[0], Line = lines[0], Order = 4 },

            new Stop { ArrivalTime = null, DepartureTime = new TimeOnly(12, 35, 0), Station = stations[1], Line = lines[1], Order = 1 },
            new Stop { ArrivalTime = new TimeOnly(12, 50, 0), DepartureTime = new TimeOnly(12, 55, 0), Station = stations[0], Line = lines[1], Order = 2 },
            new Stop { ArrivalTime = new TimeOnly(13, 10, 0), DepartureTime = null, Station = stations[4], Line = lines[1], Order = 3 }
        };

        _dbContext.Set<Stop>().AddRange(stops);

        _dbContext.SaveChanges();
    }
}