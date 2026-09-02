using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Line : BaseEntity
{
    [Required]
    public int LineNumber { get; set; }

    public virtual ICollection<Stop> Stops { get; set; } = [];

    public virtual ICollection<LineSchedule> LineSchedules { get; set; } = [];
}
