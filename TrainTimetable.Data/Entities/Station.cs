using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Station : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual Station? BaseStation { get; set; }

    public virtual Country? Country { get; set; }
}
