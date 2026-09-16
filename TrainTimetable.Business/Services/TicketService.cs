using Microsoft.IdentityModel.Tokens;
using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> FetchAllAsync();
    Task<Ticket?> FetchByIDAsync(int id);
    Task BuyAsync(TimetableItem timetableItem, ApplicationUser applicationUser, int seatCount, decimal price, PaymentMethod paymentMethod);
    Task RefundAsync(Ticket ticket);
    Task UseAsync(Ticket ticket);
    Task ExpireAsync(Ticket ticket);
    Task<bool> HasReservationAsync(TimetableItem timetableItem, ApplicationUser applicationUser);
}

public class TicketService(IBaseRepository<TicketSchedule> ticketScheduleRepository,
                           IBaseRepository<Ticket> ticketRepository) : ITicketService
{
    public async Task<IEnumerable<Ticket>> FetchAllAsync()
    {
        return await ticketRepository.GetAllAsync();
    }

    public async Task<Ticket?> FetchByIDAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        return await ticketRepository.GetByIDAsync(id);
    }

    public async Task BuyAsync(TimetableItem timetableItem, ApplicationUser applicationUser, int seatCount, decimal price, PaymentMethod paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(timetableItem);
        ArgumentNullException.ThrowIfNull(applicationUser);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seatCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(timetableItem.DepartureTime, DateTime.Now);

        TicketSchedule tempTicketSchedule;

        if (timetableItem.TicketSchedule == null)
        {
            tempTicketSchedule = await CreateTicketSchedule(timetableItem);
        } else
        {
            tempTicketSchedule = timetableItem.TicketSchedule!;
        }

        var ticket = new Ticket()
        {
            DepartureStationName = timetableItem.FirstStop!.Station.Name,
            ArrivalStationName = timetableItem.LastStop!.Station.Name,
            DepartureTime = timetableItem.DepartureTime,
            ArrivalTime = timetableItem.DepartureTime,
            SeatCount = seatCount,
            Price = price,
            PaymentMethod = paymentMethod,
            UserId = applicationUser.Id,
            TicketScheduleID = tempTicketSchedule.ID,
        };

        await ticketRepository.InsertAsync(ticket);
    }

    private async Task<TicketSchedule> CreateTicketSchedule(TimetableItem timetableItem)
    {
        var ticketSchedule = new TicketSchedule()
        {
            LineScheduleID = timetableItem.LineSchedule!.ID,
            Date = timetableItem.LineStartDate
        };

        await ticketScheduleRepository.InsertAsync(ticketSchedule);
        return ticketSchedule;
    }

    public async Task RefundAsync(Ticket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        ArgumentOutOfRangeException.ThrowIfNotEqual(ticket.TicketStatus, TicketStatus.Valid);

        ticket.TicketStatus = TicketStatus.Refunded;

        await ticketRepository.UpdateAsync(ticket);
    }

    public async Task UseAsync(Ticket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        ArgumentOutOfRangeException.ThrowIfNotEqual(ticket.TicketStatus, TicketStatus.Valid);

        ticket.TicketStatus = TicketStatus.Used;

        await ticketRepository.UpdateAsync(ticket);
    }

    public async Task ExpireAsync(Ticket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        ArgumentOutOfRangeException.ThrowIfNotEqual(ticket.TicketStatus, TicketStatus.Valid);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(ticket.ArrivalTime, DateTime.Now);

        ticket.TicketStatus = TicketStatus.Expired;

        await ticketRepository.UpdateAsync(ticket);
    }

    public async Task<bool> HasReservationAsync(TimetableItem timetableItem, ApplicationUser applicationUser)
    {
        ArgumentNullException.ThrowIfNull(timetableItem);
        ArgumentNullException.ThrowIfNull(applicationUser);

        if (timetableItem.TicketSchedule != null)
        {
            var search = await ticketRepository.BuildQueryAsync(_ =>
                _.TicketScheduleID == timetableItem.TicketSchedule.ID &&
                _.UserId == applicationUser.Id);

            if (!search.IsNullOrEmpty())
            {
                return true;
            }
        }

        return false;
    }
}
