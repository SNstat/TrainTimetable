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
        Seed<Station>(context, jsonOptions, "Stations");
        Seed<TrainManufacturer>(context, jsonOptions, "TrainManufacturers");
        Seed<Train>(context, jsonOptions, "Trains");
        Seed<Line>(context, jsonOptions, "Lines");
        Seed<LineSchedule>(context, jsonOptions, "LineSchedules");
        Seed<Stop>(context, jsonOptions, "Stops");
    }

    private static void Seed<TEntity>(AppDbContext _context, JsonSerializerOptions _options, string _fileName) where TEntity : class, IBaseEntity
    {
        if (!_context.Set<TEntity>().Any())
        {
            string filePath = String.Concat("../TrainTimetable.Data/Data/", _fileName, ".json");

            using var stream = File.OpenRead(filePath);

            var list = JsonSerializer.Deserialize<List<TEntity>>( stream, _options);

            if (list != null)
            {
                _context.Set<TEntity>().AddRange(list);
                _context.SaveChanges();
            }
        }
    }
}
