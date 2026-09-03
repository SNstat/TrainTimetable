using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ILineScheduleService
{
    Task<IEnumerable<LineItem>> FetchLineItemsAsync(int departureStationID, int arrivalStationID, DateOnly date);
}

public class LineScheduleService : ILineScheduleService
{
    private readonly IBaseRepository<LineSchedule> _lineScheduleRepository;

    public LineScheduleService(IBaseRepository<LineSchedule> lineScheduleRepository)
    {
        _lineScheduleRepository = lineScheduleRepository;
    }

    public async Task<IEnumerable<LineItem>> FetchLineItemsAsync(int departureStationID, int arrivalStationID, DateOnly date)
    {
        if (departureStationID <= 0 || arrivalStationID <= 0)
        {
            throw new ApplicationException("Argumens are invalid. ID values must be at least 1.");
        }

        var utcDateTime = DateTime.UtcNow;
        var utcDateOnly = DateOnly.FromDateTime(utcDateTime);

        if (utcDateOnly > date)
        {
            throw new ApplicationException("Argument date is invalid. Date cant reference past date.");
        }

        var drivingDays = date.ToDrivingDays();

        var lineSchedules = _lineScheduleRepository.BuildQuery(
            _ =>
            _.DriveDays.HasFlag(drivingDays) &&
            _.Line.Stops.Any(dep => dep.StationID == departureStationID) &&
            _.Line.Stops.Any(arr => arr.StationID == arrivalStationID) &&
            _.Line.Stops.Where(dep => dep.StationID == departureStationID).Select(dep => dep.Order).FirstOrDefault() <
            _.Line.Stops.Where(arr => arr.StationID == arrivalStationID).Select(arr => arr.Order).FirstOrDefault(),
            ls => ls
                .Include(_ => _.Train)
                .Include(_ => _.Line)
                .ThenInclude(_ => _.Stops)
                .ThenInclude(_ => _.Station)
            );

        if (lineSchedules.IsNullOrEmpty())
        {
            return (IEnumerable<LineItem>)[];
        }

        var lineItems = new List<LineItem>();

        foreach (var lineSchedule in lineSchedules)
        {
            var lineScheduleStartTime = date.ToDateTime(lineSchedule.StartTime);

            if(utcDateTime > lineScheduleStartTime) // Skips the schedules that have passed today
                continue;

            var departureStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.StationID == departureStationID);

            var arrivalStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.StationID == arrivalStationID);

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
                    Price = (arrivalTime - departureTime).ToPrice()
                });
            }
        }

        return lineItems;
    }
}
