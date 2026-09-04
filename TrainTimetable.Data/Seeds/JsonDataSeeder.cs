using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Seeds;

internal class JsonDataSeeder
{
    internal static void SeedDevelopmentData(DbContext _dbContext)
    {
        var context = _dbContext as AppDbContext;

        if (context == null)
            throw new ArgumentNullException("Argument dbContext is invalid. The context must be not null.");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        Seed<Country>(context, jsonOptions, "Countries");

        Seed<Station>(context, jsonOptions, "Stations", (T, db) =>
        {
            T.CountryID = db.Countries.First(_ => _.Name == T.Country.Name).ID;
            T.Country = null!;
        });

        Seed<TrainManufacturer>(context, jsonOptions, "TrainManufacturers");

        Seed<Train>(context, jsonOptions, "Trains", (T, db) => {
            T.TrainManufacturerID = db.TrainManufacturers.First(_ => _.Name == T.TrainManufacturer.Name).ID;
            T.TrainManufacturer = null!;
        });

        Seed<Line>(context, jsonOptions, "Lines");

        Seed<LineSchedule>(context, jsonOptions, "LineSchedules",
            (T, db) =>
            {
                T.LineID = db.Lines.First(_ => _.LineNumber == T.Line.LineNumber).ID;
                T.Line = null!;
            },
            (T, db) =>
            {
                T.TrainID = db.Trains.First(_ => _.TrainNumber == T.Train.TrainNumber).ID;
                T.Train = null!;
            });

        Seed<Stop>(context, jsonOptions, "Stops",
            (T, db) =>
            {
                T.StationID = db.Stations.First(_ => _.Name == T.Station.Name).ID;
                T.Station = null!;
            },
            (T, db) =>
            {
                T.LineID = db.Lines.First(_ => _.LineNumber == T.Line.LineNumber).ID;
                T.Line = null!;
            });
    }

    private static void Seed<TEntity>(AppDbContext _context, JsonSerializerOptions _options, string _fileName,
        params ICollection<Action<TEntity, AppDbContext>> referenceHandlers) where TEntity : class, IBaseEntity
    {
        if (!_context.Set<TEntity>().Any())
        {
            string filePath = String.Concat("../TrainTimetable.Data/Data/", _fileName, ".json");

            using var stream = File.OpenRead(filePath);

            var list = JsonSerializer.Deserialize<List<TEntity>>(stream, _options);

            if (list != null)
            {
                foreach (var entity in list)
                {
                    foreach (var referenceHandler in referenceHandlers)
                    {
                        referenceHandler(entity, _context);
                    }
                }
                _context.Set<TEntity>().AddRange(list);
                _context.SaveChanges();
            }
        }
    }
}
