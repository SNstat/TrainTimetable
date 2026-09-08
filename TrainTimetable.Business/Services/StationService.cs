using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface IStationService
{
    Task<IEnumerable<StationItem>> FetchStationsBySearchAsync(string search);
    Task<IEnumerable<StationItem>> FetchFirstTenStationsAsync();
}

public class StationService(IBaseRepository<Station> _baseRepository) : IStationService
{
    public async Task<IEnumerable<StationItem>> FetchStationsBySearchAsync(string search)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(search);

        var stations = await _baseRepository
            .BuildQueryAsync(_ => _.Name.ToLower().StartsWith(search.ToLower()));

        if (stations.IsNullOrEmpty())
        {
            return Enumerable.Empty<StationItem>();
        }

        return stations
            .OrderBy(_ => _.Name)
            .Take(10)
            .Select(_ => new StationItem(_.ID, _.Name));
    }

    public async Task<IEnumerable<StationItem>> FetchFirstTenStationsAsync()
    {
        var stations = await _baseRepository.GetAllAsync();
        return stations.OrderBy(_ => _.Name)
            .Take(10)
            .Select(_ => new StationItem(_.ID, _.Name));
    }
}