using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Models;

namespace TrainTimetable.Data.Entities;

public class Line : BaseEntity
{
    [Required]
    public int LineNumber { get; set; }

    [Required]
    public DrivingDays DriveDays { get; set; } = DrivingDays.NotActive;

    public virtual ICollection<Stop> Stops { get; set; } = [];

    public virtual ICollection<LineSchedule> LineSchedules { get; set; } = [];
}
