using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ILineService
{

}

public class LineService(IBaseRepository<Line> lineRepository) : ILineService
{
    private readonly IBaseRepository<Line> _lineRepository = lineRepository;


}
