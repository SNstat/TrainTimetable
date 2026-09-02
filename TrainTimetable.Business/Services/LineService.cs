using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ILineService
{
    Task<IEnumerable<LineItem>> FetchAllLineItems();
}

public class LineService(IBaseRepository<Line> lineRepository) : ILineService
{
    private readonly IBaseRepository<Line> _lineRepository = lineRepository;

    public Task<IEnumerable<LineItem>> FetchAllLineItems()
    {
        throw new NotImplementedException();
    }
}
