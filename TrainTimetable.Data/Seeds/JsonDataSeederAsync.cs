using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Seeds;

internal class JsonDataSeederAsync
{
    internal static void SeedDevelopmentData(DbContext _dbContext)
    {
        SeedDevelopmentDataAsync(_dbContext).GetAwaiter().GetResult();
    }

    internal static async Task SeedDevelopmentDataAsync(DbContext _dbContext)
    {
        var context = _dbContext as AppDbContext;

        if (context == null)
            throw new ArgumentNullException("Argument dbContext is invalid. The context must be not null.");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        await SeedAsync<Country>(context, jsonOptions, "Countries");

        await SeedAsync<Station>(context, jsonOptions, "Stations", (T, db) =>
        {
            T.CountryID = db.Countries.First(_ => _.Name == T.Country.Name).ID;
            T.Country = null!;
        });

        await SeedAsync<TrainManufacturer>(context, jsonOptions, "TrainManufacturers");

        await SeedAsync<Train>(context, jsonOptions, "Trains", (T, db) => {
            T.TrainManufacturerID = db.TrainManufacturers.First(_ => _.Name == T.TrainManufacturer.Name).ID;
            T.TrainManufacturer = null!;
        });

        await SeedAsync<Line>(context, jsonOptions, "Lines");

        await SeedAsync<LineSchedule>(context, jsonOptions, "LineSchedules",
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

        await SeedAsync<Stop>(context, jsonOptions, "Stops",
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

    private async static Task SeedAsync<TEntity>(AppDbContext _context, JsonSerializerOptions _options, string _fileName,
        params Action<TEntity, AppDbContext>[] referenceHandlers) where TEntity : class, IBaseEntity
    {
        if (!await _context.Set<TEntity>().AnyAsync())
        {
            string filePath = String.Concat("../TrainTimetable.Data/Data/", _fileName, ".json");

            await using var stream = File.OpenRead(filePath);

            var list = await JsonSerializer.DeserializeAsync<List<TEntity>>( stream, _options);

            if (list != null)
            {
                foreach (var entity in list)
                {
                    foreach (var referenceHandler in referenceHandlers)
                    {
                        referenceHandler(entity, _context);
                    }
                }

                await _context.Set<TEntity>().AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
