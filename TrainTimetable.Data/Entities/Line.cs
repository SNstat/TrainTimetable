using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class Line : BaseEntity
{
    [Required]
    public int LineNumber { get; set; }

    public int TrainID { get; set; }
    [ForeignKey(nameof(TrainID))]
    public Train? Train { get; set; }

    public virtual ICollection<Stop> Stops { get; set; } = [];
}
