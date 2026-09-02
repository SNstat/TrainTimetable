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

public class Ticket
{
    [Required]
    public int DepartureStationID { get; set; }
    public required virtual Station DepartureStation { get; set; }

    [Required]
    public int ArrivalStationID { get; set; }
    public required virtual Station ArrivalStation { get; set; }

    [Required]
    public TimeOnly DepartureTime { get; set; }

    [Required]
    public TimeOnly ArrivalTime { get; set; }

    [Required]
    public int SeatCount { get; set; } = 1;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public TicketStatus TicketStatus { get; set; } = TicketStatus.Valid;

    /*
    [Required]
    public int TrainTimetableUserID { get; set; }
    public required virtual TrainTimetableUser TrainTimetableUser { get; set; }
    */
}
