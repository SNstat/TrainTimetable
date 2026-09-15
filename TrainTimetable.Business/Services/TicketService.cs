using TrainTimetable.Business.Models;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> FetchAllAsync();
    Task<Ticket?> FetchByIDAsync(int id);
    Task BuyAsync(TimetableItem timetableItem, ApplicationUser applicationUser, int seatCount, DateTime date, decimal price);
    Task RefundAsync(Ticket ticket);
    Task UseAsync(Ticket ticket);
    Task ExpireAsync(Ticket ticket);
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

    public async Task BuyAsync(TimetableItem timetableItem, ApplicationUser applicationUser, int seatCount, DateTime date, decimal price)
    {
        ArgumentNullException.ThrowIfNull(timetableItem);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seatCount);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(date, DateTime.Now);

        if (timetableItem.TicketSchedule == null && timetableItem.LineSchedule != null)
        {
            await CreateTicketSchedule(timetableItem.LineSchedule, date);
        }

        var ticket = new Ticket()
        {
            DepartureStation = timetableItem.FirstStop!.Station,
            ArrivalStation = timetableItem.LastStop!.Station,
            DepartureTime = timetableItem.DepartureTime,
            ArrivalTime = timetableItem.DepartureTime,
            SeatCount = seatCount,
            Price = price,
            TicketSchedule = timetableItem.TicketSchedule!,
            ApplicationUser = applicationUser
        };

        await ticketRepository.InsertAsync(ticket);
    }

    private async Task CreateTicketSchedule(LineSchedule lineSchedule, DateTime date)
    {
        var ticketSchedule = new TicketSchedule()
        {
            LineSchedule = lineSchedule,
            Date = date
        };

        await ticketScheduleRepository.InsertAsync(ticketSchedule);
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
}
