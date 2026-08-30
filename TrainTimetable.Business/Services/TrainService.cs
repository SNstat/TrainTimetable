using Microsoft.EntityFrameworkCore;
using TrainTimetable.Data;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{

}

public class TrainService : ITrainService
{
    private readonly IBaseRepository<Train> _trainRepository;

    public TrainService(IBaseRepository<Train> trainRepository)
    {
        _trainRepository = trainRepository;
    }


}
