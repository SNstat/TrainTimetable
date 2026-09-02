using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class LineSchedule : BaseEntity
{
    public int LineID { get; set; }
    [ForeignKey(nameof(LineID))]
    public virtual Line? Line { get; set; }

    public int TrainID { get; set; }
    [ForeignKey(nameof(TrainID))]
    public Train? Train { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }
}
