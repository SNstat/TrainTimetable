using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{
    Task RegisterAsync(Train train);

    Task UpdateInfoAsync(Train train);

    Task RemoveAsync(Train train);

    Task<Train?> FetchByIdAsync(int id);

    Task<IEnumerable<Train>> FetchAllAsync();

    Task<IEnumerable<Train>> FetchAllActiveAsync();

    Task<IEnumerable<Train>> FetchAllInactiveAsync();

    Task<IEnumerable<TrainManufacturer>> FetchAllTrainManufacturersAsync();

    Task AddTrainManufacturerAsync(string name);
}

public class TrainService(
    IBaseRepository<Train> trainRepository,
    IBaseRepository<TrainManufacturer> trainManufacturerRepository) : ITrainService
{
    private static async Task ValidateTrain(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (string.IsNullOrWhiteSpace(train.Name) || train.Name.Length > 250)
        {
            throw new ApplicationException("Invalid name. Name must not be null or empty and must be between 1 and 250 characters long.");
        }

        if (train.SeatCount < 1 || train.SeatCount > 1000)
        {
            throw new ApplicationException("Invalid number of seats. Valid range is 1 to 1000.");
        }

        if (train.TrainManufacturerID < 1)
        {
            throw new ApplicationException("Invalid manufacturer ID. Manufacturer ID must be at least 1.");
        }

        if (train.BikeSpaceCount < 0)
        {
            throw new ApplicationException("Invalid bike space count. Bike space count cannot be lower than 0.");
        }

        if (train.DisabledSeatCount < 0 || train.DisabledSeatCount > train.SeatCount)
        {
            throw new ApplicationException("Invalid seat count. Seat count cannot be lower than 0 nor higher than the seat count.");
        }
    }

    public async Task RegisterAsync(Train train)
    {
        await ValidateTrain(train);

        await trainRepository.InsertAsync(train);
    }

    public async Task UpdateInfoAsync(Train train)
    {
        await ValidateTrain(train);

        await trainRepository.UpdateAsync(train);
    }

    public async Task RemoveAsync(Train train)
    {
        await trainRepository.DeleteAsync(train);
    }

    public async Task<Train?> FetchByIdAsync(int id)
    {
        if (id <= 0)
            throw new ApplicationException("Invalid search ID. ID must be at least 1.");

        return await trainRepository.GetByIDAsync(id);
    }

    public async Task<IEnumerable<Train>> FetchAllAsync()
    {
        var query = await trainRepository.BuildQueryAsync(_ => true,
            _ => _.Include(_ => _.TrainManufacturer));

        return query ?? [];
    }

    public async Task<IEnumerable<Train>> FetchAllActiveAsync()
    {
        var query = await trainRepository.BuildQueryAsync(_ => _.IsActive == true,
            _ => _.Include(_ => _.TrainManufacturer));

        return query;
    }

    public async Task<IEnumerable<Train>> FetchAllInactiveAsync()
    {
        var query = await trainRepository.BuildQueryAsync(_ => _.IsActive == false,
            _ => _.Include(_ => _.TrainManufacturer));

        return query;
    }

    public async Task<IEnumerable<TrainManufacturer>> FetchAllTrainManufacturersAsync()
    {
        return await trainManufacturerRepository.GetAllAsync() ?? [];
    }

    public async Task AddTrainManufacturerAsync(string name)
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
            }
        }
    }
}
