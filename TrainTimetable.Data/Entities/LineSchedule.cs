using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Models;

namespace TrainTimetable.Data.Entities;

public class LineSchedule : BaseEntity
{
    [Required]
    public int LineID { get; set; }
    [Required]
    public virtual Line Line { get; set; } = new Line();

    [Required]
    public int TrainID { get; set; }
    [Required]
    public virtual Train Train { get; set; } = new Train();

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public DrivingDays DriveDays { get; set; } = DrivingDays.NotActive;
}
