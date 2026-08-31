using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{
    Task RegisterTrainAsync(Train train);
    Task UpdateTrainAsync(Train train);
    Task<Train?> GetTrainByIdAsync(int id);
    Task<IEnumerable<Train>> GetAllActiveTrainsAsync();
    Task<IEnumerable<Train>> GetAllInactiveTrainsAsync();
}

public class TrainService : ITrainService
{
    private readonly IBaseRepository<Train> _trainRepository;

    public TrainService(IBaseRepository<Train> trainRepository)
    {
        _trainRepository = trainRepository;
    }

    public async Task RegisterTrainAsync(Train train)
    {
        await _trainRepository.AddAsync(train);
    }

    public async Task UpdateTrainAsync(Train train)
    {
        await _trainRepository.UpdateAsync(train);
    }

    public async Task<Train?> GetTrainByIdAsync(int id)
    {
        return await _trainRepository.GetByIDAsync(id);
    }

    public async Task<IEnumerable<Train>> GetAllActiveTrainsAsync()
    {
        return await _trainRepository.FindAsync(_ => _.IsActive);
    }

    public async Task<IEnumerable<Train>> GetAllInactiveTrainsAsync()
    {
        return await _trainRepository.FindAsync(_ => !_.IsActive);
    }
}
