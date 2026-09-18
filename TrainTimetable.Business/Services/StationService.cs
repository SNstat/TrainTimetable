using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface IStationService
{
    Task<IEnumerable<KeyValuePair<int, string>>> FetchStationItemsAsync(string search);
    Task<IEnumerable<Station>> FetchAllAsync();
}

public class StationService(IBaseRepository<Station> stationRepository) : IStationService
{
    public async Task<IEnumerable<KeyValuePair<int, string>>> FetchStationItemsAsync(string search)
    {
        ArgumentNullException.ThrowIfNull(search);

        IEnumerable<Station> stations;

        if (search == String.Empty)
        {
            stations = await stationRepository.GetAllAsync();
        } else
        {
            stations = await stationRepository
                .BuildQueryAsync(_ => _.Name.ToLower().StartsWith(search.ToLower()));
        }

        if (stations.IsNullOrEmpty())
        {
            return [];
        }

        return stations
            .OrderBy(_ => _.Name)
            .Take(10)
            .Select(_ => new KeyValuePair<int, string>(_.ID, _.Name));
    }

    public async Task<IEnumerable<Station>> FetchAllAsync()
    {
        var query = await stationRepository.BuildQueryAsync(_ => true,
            _ => _.Include(_ => _.Country));

        return query ?? [];
    }
}