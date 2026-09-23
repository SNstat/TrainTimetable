using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainManufacturerService
{
    Task<IEnumerable<TrainManufacturer>> FetchTrainManufacturerItemsAsync(string search);
}

public class TrainManufacturerService(IBaseRepository<TrainManufacturer> trainManufacturerRepository) : ITrainManufacturerService
{
    public async Task<IEnumerable<TrainManufacturer>> FetchTrainManufacturerItemsAsync(string search)
    {
        ArgumentNullException.ThrowIfNull(search);

        IEnumerable<TrainManufacturer> trainManufacturers;

        if (search == String.Empty)
        {
            trainManufacturers = await trainManufacturerRepository.GetAllAsync();
        }
        else
        {
            trainManufacturers = await trainManufacturerRepository
                .BuildQueryAsync(_ => _.Name.ToLower().StartsWith(search.ToLower()));
        }

        if (trainManufacturers.IsNullOrEmpty())
        {
            return [];
        }

        return trainManufacturers
            .OrderBy(_ => _.Name)
            .Take(10);
    }
}
