using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ILineService
{
    Task<IEnumerable<LineItem>?> FetchLineItems(Station departureStation, Station arrivalStation, DateOnly date);
}

public class LineService : ILineService
{
    private readonly IBaseRepository<LineSchedule> _lineScheduleRepository;

    public LineService(IBaseRepository<LineSchedule> lineScheduleRepository)
    {
        _lineScheduleRepository = lineScheduleRepository;
    }

    public async Task<IEnumerable<LineItem>?> FetchLineItems(Station departureStation, Station arrivalStation, DateOnly date)
    {
        ArgumentNullException.ThrowIfNull(departureStation);
        ArgumentNullException.ThrowIfNull(arrivalStation);

        var utcDateTime = DateTime.UtcNow;
        var utcDateOnly = DateOnly.FromDateTime(utcDateTime);

        if (utcDateOnly > date)
        {
            throw new ApplicationException("Argument date is invalid. Date cant reference past date.");
        }

        var drivingDays = date.ToDrivingDays();

        Expression<Func<LineSchedule, bool>> predicateExpression = _ =>
            _.DriveDays == drivingDays &&
            _.Line.Stops.Any(dep => dep.Station == departureStation &&
            _.Line.Stops.Any(arr => arr.Station == arrivalStation &&
            dep.Order < arr.Order));     

        var lineSchedules = await _lineScheduleRepository.FilterAsync(predicateExpression);

        if (lineSchedules.IsNullOrEmpty())
        {
            return null;
        }

        var lineItems = new List<LineItem>();

        foreach (var lineSchedule in lineSchedules)
        {
            var lineScheduleStartTime = utcDateOnly.ToDateTime(lineSchedule.StartTime);

            if(utcDateTime >  lineScheduleStartTime)
            {
                continue;
            }

            var departureStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.Station == departureStation);

            var arrivalStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.Station == arrivalStation);

            if (departureStop != null && arrivalStop != null)
            {
                var stopSubset = lineSchedule.Line.Stops
                    .Where(_ => _.Order >= departureStop.Order && _.Order <= arrivalStop.Order)
                    .OrderBy(_ => _.Order)
                    .ToList();

                var departureTime = lineScheduleStartTime + (departureStop.DepartureOffset ?? TimeSpan.Zero);
                var arrivalTime = lineScheduleStartTime + (arrivalStop.ArrivalOffset ?? TimeSpan.Zero);

                lineItems.Add(new()
                {
                    Stops = stopSubset,
                    Train = lineSchedule.Train,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Price = (departureTime - arrivalTime).ToPrice()
                });
            }
        }

        return lineItems;
    }
}
