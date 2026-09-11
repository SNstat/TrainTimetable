using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface IStationService
{
    Task<IEnumerable<KeyValuePair<int, string>>> FetchStationItemsAsync(string search);
}

public class StationService(IBaseRepository<Station> baseRepository) : IStationService
{
    public async Task<IEnumerable<KeyValuePair<int, string>>> FetchStationItemsAsync(string search)
    {
        ArgumentNullException.ThrowIfNull(search);

        IEnumerable<Station> stations;

        if (search == String.Empty)
        {
            stations = await baseRepository.GetAllAsync();
        } else
        {
            stations = await baseRepository
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
}