using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{
    Task RegisterAsync(Train train);

    Task UpdateInfoAsync(Train train);

    Task<Train?> FetchByIdAsync(int id);

    Task<IEnumerable<Train>> FetchAllAsync();

    Task<IEnumerable<Train>> FetchAllActiveAsync();

    Task<IEnumerable<Train>> FetchAllInactiveAsync();
}

public class TrainService(
    IBaseRepository<Train> trainRepository) : ITrainService
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

    public async Task<Train?> FetchByIdAsync(int id)
    {
        if (id <= 0)
            throw new ApplicationException("Invalid search ID. ID must be at least 1.");

        return await trainRepository.GetByIDAsync(id);
    }

    public async Task<IEnumerable<Train>> FetchAllAsync()
    {
        return await trainRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Train>> FetchAllActiveAsync()
    {
        var query = trainRepository.BuildQuery(_ => _.IsActive);
        return query.ToList();
    }

    public async Task<IEnumerable<Train>> FetchAllInactiveAsync()
    {
        var query = trainRepository.BuildQuery(_ => !_.IsActive);
        return query.ToList();
    }
}
