using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainManufacturerService
{
    Task<IEnumerable<TrainManufacturer>> FetchTrainManufacturerItemsAsync(string search);

    Task<int> IdentifyTrainManufacturerAsync(string name);
}

public class TrainManufacturerService(IBaseRepository<TrainManufacturer> trainManufacturerRepository) : ITrainManufacturerService
{
    public async Task<IEnumerable<TrainManufacturer>> FetchTrainManufacturerItemsAsync(string search)
    {
        ArgumentNullException.ThrowIfNull(search);

        IEnumerable<TrainManufacturer> manufacturers;

        if (search == String.Empty)
        {
            manufacturers = await trainManufacturerRepository.GetAllAsync();
        }
        else
        {
            manufacturers = await trainManufacturerRepository
                .BuildQueryAsync(_ => _.Name.ToLower().StartsWith(search.ToLower()));
        }

        if (manufacturers.IsNullOrEmpty())
        {
            return [];
        }

        return manufacturers
            .OrderBy(_ => _.Name)
            .Take(10);
    }

    public async Task<int> IdentifyTrainManufacturerAsync(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var query = await trainManufacturerRepository.BuildQueryAsync(_ => _.Name == name);

            if (query.IsNullOrEmpty())
            {
                var trainManufacturer = new TrainManufacturer()
                {
                    Name = name
                };

                await trainManufacturerRepository.InsertAsync(trainManufacturer);

                query = await trainManufacturerRepository.BuildQueryAsync(_ => _.Name == name);
            }

            return query.FirstOrDefault().ID;
        }

        return 0;
    }
}
