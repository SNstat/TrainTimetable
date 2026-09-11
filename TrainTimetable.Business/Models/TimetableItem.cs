using TrainTimetable.Data.Entities;

namespace TrainTimetable.Business.Models;

public record TimetableItem
{
    public int ID { get; set; }

    public List<Stop>? Stops { get; set; }

    public Train? Train { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public decimal Price { get; set; } = 0;

    public bool ShowDetails { get; set; }

    // calculated

    public TimeSpan? TripDuration => ArrivalTime - DepartureTime;

    public Stop? FirstStop => Stops?.MinBy(_ => _.Order);

    public Stop? LastStop => Stops?.MaxBy(_ => _.Order);

    public Station? DepartureStation => FirstStop?.Station;

    public Station? ArrivalStation => LastStop?.Station;
}
