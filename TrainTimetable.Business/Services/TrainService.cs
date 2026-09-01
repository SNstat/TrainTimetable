using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{
    Task RegisterAsync(Train train);

    Task UpdateInfoAsync(Train train);

    Task<Train?> GetByIdAsync(int id);

    Task<IEnumerable<Train>> GetAllActiveAsync();

    Task<IEnumerable<Train>> GetAllInactiveAsync();
}

public class TrainService(
    IBaseRepository<Train> trainRepository) : ITrainService
{
    private readonly IBaseRepository<Train> _trainRepository = trainRepository;

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

        await _trainRepository.InsertAsync(train);
    }

    public async Task UpdateInfoAsync(Train train)
    {
        await ValidateTrain(train);

        await _trainRepository.UpdateAsync(train);
    }

    public async Task<Train?> GetByIdAsync(int id)
    {
        return await _trainRepository.GetByIDAsync(id);
    }

    public async Task<IEnumerable<Train>> GetAllActiveAsync()
    {
        return await _trainRepository.FilterAsync(_ => _.IsActive);
    }

    public async Task<IEnumerable<Train>> GetAllInactiveAsync()
    {
        return await _trainRepository.FilterAsync(_ => !_.IsActive);
    }
}
