using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Models;

namespace TrainTimetable.Data.Entities;

public class LineSchedule : BaseEntity
{
    [Required]
    public int LineID { get; set; }
    public required virtual Line Line { get; set; }

    [Required]
    public int TrainID { get; set; }
    public required virtual Train Train { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public DrivingDays DriveDays { get; set; } = DrivingDays.NotActive;
}
