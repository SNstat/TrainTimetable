using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class TicketSchedule : BaseEntity
{
    [Required]
    public int LineScheduleID { get; set; }
    public required virtual LineSchedule LineSchedule { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    public virtual IEnumerable<Ticket> Tickets { get; set; } = [];

    [NotMapped]
    public int AvailableSeatCount => LineSchedule.Train.SeatCount - Tickets
        .Where(_ => _.TicketStatus == TicketStatus.Valid || _.TicketStatus == TicketStatus.Used)
        .Sum(_ => _.SeatCount);

    [NotMapped]
    public bool IsFull => AvailableSeatCount == 0;
}
