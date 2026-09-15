using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class TicketSchedule : BaseEntity
{
    [Required]
    public int LineScheduleID { get; set; }
    [Required]
    public virtual LineSchedule LineSchedule { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = [];

    [NotMapped]
    public int ReservedSeatCount => Tickets
        .Where(_ => _.TicketStatus == TicketStatus.Valid || _.TicketStatus == TicketStatus.Used)
        .Sum(_ => _.SeatCount);

    [NotMapped]
    public int AvailableSeatCount => LineSchedule.Train.SeatCount - ReservedSeatCount;

    [NotMapped]
    public bool IsFull => AvailableSeatCount == 0;
}
