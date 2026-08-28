using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Line
{
    [Key]
    public int ID { get; set; }

    public int LineNumber { get; set; }

    [Required]
    public Stop StartSegment { get; set; } = null!;

    [Required]
    public virtual Stop? FirstStop { get; set; }

    public virtual IEnumerable<Stop> Stops { get; set; } = [];
}
