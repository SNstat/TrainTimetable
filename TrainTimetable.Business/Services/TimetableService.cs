using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITimetableService
{
    Task<IEnumerable<TimetableItem>> FetchLineItemsAsync(int departureStationID, int arrivalStationID, DateOnly date);
}

public class TimetableService(IBaseRepository<LineSchedule> lineScheduleRepository) : ITimetableService
{
    private readonly IBaseRepository<LineSchedule> _lineScheduleRepository = lineScheduleRepository;

    public async Task<IEnumerable<TimetableItem>> FetchLineItemsAsync(int departureStationID, int arrivalStationID, DateOnly date)
    {
        if (departureStationID <= 0 || arrivalStationID <= 0)
        {
            throw new ApplicationException("Argumens are invalid. ID values must be at least 1.");
        }

        var currentDateTime = DateTime.Now;
        var currentDateOnly = DateOnly.FromDateTime(currentDateTime);

        if (currentDateOnly > date)
        {
            throw new ApplicationException("Argument date is invalid. Date cant reference past date.");
        }

        var drivingDays = date.ToDrivingDays();

        var lineSchedules = await _lineScheduleRepository.BuildQueryAsync(
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
            return Enumerable.Empty<TimetableItem>();
        }

        var timetableItems = new List<TimetableItem>();

        foreach (var lineSchedule in lineSchedules)
        {
            var lineScheduleStartTime = date.ToDateTime(lineSchedule.StartTime);

            var departureStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.StationID == departureStationID);

            var arrivalStop = lineSchedule.Line.Stops
                .FirstOrDefault(_ => _.StationID == arrivalStationID);

            if (departureStop != null && arrivalStop != null)
            {
                var stopSubset = lineSchedule.Line.Stops
                    .Where(_ => _.Order >= departureStop.Order && _.Order <= arrivalStop.Order);

                var normalizedStops = stopSubset
                    .Select(_ => new Stop
                    {
                        StationID = _.StationID,
                        Order = _.Order,
                        Station = _.Station,
                        DepartureOffset = (_.DepartureOffset ?? TimeSpan.Zero) - (departureStop.DepartureOffset ?? TimeSpan.Zero),
                        ArrivalOffset = (_.ArrivalOffset ?? TimeSpan.Zero) - (departureStop.DepartureOffset ?? TimeSpan.Zero)
                    })
                    .OrderBy(_ => _.Order)
                    .ToList();

                var departureTime = lineScheduleStartTime + (departureStop.DepartureOffset ?? TimeSpan.Zero);
                var arrivalTime = lineScheduleStartTime + (arrivalStop.ArrivalOffset ?? TimeSpan.Zero);

                if (currentDateTime > departureTime) // Skips the schedules that have passed today at the specific departure station
                    continue;

                timetableItems.Add(new()
                {
                    ID = lineSchedule.ID,
                    Stops = normalizedStops,
                    Train = lineSchedule.Train,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Price = (arrivalTime - departureTime).ToPrice()
                });
            }
        }

        return timetableItems.OrderBy(_ => _.DepartureTime);
    }
}
