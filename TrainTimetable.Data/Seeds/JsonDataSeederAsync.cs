using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Seeds;

internal class JsonDataSeederAsync
{
    internal static async Task SeedDevelopmentDataAsync(DbContext _dbContext)
    {
        var context = _dbContext as AppDbContext;

        if (context == null)
            throw new ArgumentNullException("Argument dbContext is invalid. The context must be not null.");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        await Seed<Country>(context, jsonOptions, "Countries");
        await Seed<Station>(context, jsonOptions, "Stations");
        await Seed<TrainManufacturer>(context, jsonOptions, "TrainManufacturers");
        await Seed<Train>(context, jsonOptions, "Trains");
        await Seed<Line>(context, jsonOptions, "Lines");
        await Seed<LineSchedule>(context, jsonOptions, "LineSchedules");
        await Seed<Stop>(context, jsonOptions, "Stops");
    }

    private async static Task Seed<TEntity>(AppDbContext _context, JsonSerializerOptions _options, string _fileName) where TEntity : class, IBaseEntity
    {
        if (!await _context.Set<TEntity>().AnyAsync())
        {
            string filePath = String.Concat("../TrainTimetable.Data/Data/", _fileName, ".json");

            await using var stream = File.OpenRead(filePath);

            var list = await JsonSerializer.DeserializeAsync<List<TEntity>>( stream, _options);

            if (list != null)
            {
                await _context.Set<TEntity>().AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
