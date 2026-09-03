using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public enum TicketStatus
{
    Valid, // Purchased ticket that can be used at the specific time schedule. Defines available seats.
    Used, // Used purchased ticket at the specific time schedule. Not usable.
    Refunded, // Refunded unused ticket. Not usable.
    Expired // Expired unused ticket. Not usable
}

public class Ticket : BaseEntity
{
    [Required]
    public string DepartureStationName { get; set; } = String.Empty;

    [Required]
    public string ArrivalStationName { get; set; } = String.Empty;

    [Required]
    public DateTime DepartureTime { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }

    [Required]
    public int SeatCount { get; set; } = 1;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public TicketStatus TicketStatus { get; set; } = TicketStatus.Valid;

    [Required]
    public int TicketScheduleID { get; set; }
    [Required]
    public virtual TicketSchedule TicketSchedule { get; set; }

    /*
    [Required]
    public int TrainTimetableUserID { get; set; }
    public required virtual TrainTimetableUser TrainTimetableUser { get; set; }
    */
}
