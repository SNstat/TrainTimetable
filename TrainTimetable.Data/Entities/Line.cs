using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Line : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int LineNumber { get; set; }

    public virtual ICollection<Stop> Stops { get; set; } = [];

    public int TrainID { get; set; }
    [ForeignKey(nameof(TrainID))]
    public Train? Train { get; set; }
}
