using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Country : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}
