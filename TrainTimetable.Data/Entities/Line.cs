using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Line : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int LineNumber { get; set; }

    public virtual ICollection<Stop> Stops { get; set; } = [];
}
