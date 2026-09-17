using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Models;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITimetableService
{
    Task<IEnumerable<TimetableItem>> FetchTimetableItemsAsync(int departureStationID, int arrivalStationID, DateOnly date, int seatCount);
}

public class TimetableService(IBaseRepository<LineSchedule> lineScheduleRepository, IPaymentService PaymentService) : ITimetableService
{
    private readonly IBaseRepository<LineSchedule> _lineScheduleRepository = lineScheduleRepository;

    public async Task<IEnumerable<TimetableItem>> FetchTimetableItemsAsync(int departureStationID, int arrivalStationID, DateOnly date, int seatCount)
    {
        if (departureStationID <= 0 || arrivalStationID <= 0)
        {
            throw new ApplicationException("Argumens are invalid. ID values must be at least 1.");
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(seatCount, 1);

        var currentDateTime = DateTime.Now;
        var currentDateOnly = DateOnly.FromDateTime(currentDateTime);

        if (currentDateOnly > date)
        {
            throw new ApplicationException("Argument date is invalid. Date cant reference past date.");
        }

        var drivingDays = date.ToDrivingDays();

        var baseLineSchedules = await _lineScheduleRepository.BuildQueryAsync(
            _ => _.DriveDays.HasFlag(drivingDays) &&
                  _.Line.Stops.Any(dep => dep.StationID == departureStationID) &&
                  _.Line.Stops.Any(arr => arr.StationID == arrivalStationID) &&
                  _.Line.Stops.Where(dep => dep.StationID == departureStationID).Select(dep => dep.Order).FirstOrDefault() <
                  _.Line.Stops.Where(arr => arr.StationID == arrivalStationID).Select(arr => arr.Order).FirstOrDefault(),
            _ => _
                .Include(_ => _.Train)
                .Include(_ => _.Line)
                    .ThenInclude(_ => _.Stops)
                    .ThenInclude(_ => _.Station)
                .Include(_ => _.TicketSchedules)
                    .ThenInclude(_ => _!.Tickets)
        );

        var lineSchedules = baseLineSchedules.Where(_ =>
        {
            var departureStop = _.Line.Stops.FirstOrDefault(st => st.StationID == departureStationID);
            var offsetDays = (departureStop?.DepartureOffset ?? TimeSpan.Zero).Days;
            var targetDate = date.AddDays(-offsetDays);

            var seats = _.TicketSchedules
                .Where(ts => ts != null && ts.Date == targetDate)
                .SelectMany(ts => ts!.Tickets)
                .Where(t => t.TicketStatus == TicketStatus.Valid || t.TicketStatus == TicketStatus.Used)
                .Sum(t => t.SeatCount);

            return (_.Train.SeatCount - seats) >= seatCount;
        }).ToList();

        if (lineSchedules.IsNullOrEmpty())
        {
            return [];
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

                var price = PaymentService.CalculatePrice(arrivalTime - departureTime, seatCount);

                var lineScheduleStartDate = date.AddDays(-(departureStop.DepartureOffset ?? TimeSpan.Zero).Days);

                var ticketSchedule = lineSchedule.TicketSchedules.FirstOrDefault(_ => _!.Date == lineScheduleStartDate, null);

                timetableItems.Add(new()
                {
                    ID = lineSchedule.ID,
                    Stops = normalizedStops,
                    Train = lineSchedule.Train,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Price = price,
                    SeatCount = seatCount,
                    LineSchedule = lineSchedule,
                    LineStartDate = lineScheduleStartDate,
                    TicketSchedule = ticketSchedule
                });
            }
        }

        return timetableItems.OrderBy(_ => _.DepartureTime);
    }
}
