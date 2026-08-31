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

public class TrainService : ITrainService
{
    private readonly IBaseRepository<Train> _trainRepository;

    public TrainService(IBaseRepository<Train> trainRepository)
    {
        _trainRepository = trainRepository;
    }

    public async Task RegisterAsync(Train train)
    {
        await _trainRepository.AddAsync(train);
    }

    public async Task UpdateInfoAsync(Train train)
    {
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
