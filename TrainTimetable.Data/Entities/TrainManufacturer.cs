using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class TrainManufacturer : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = String.Empty;

    public virtual ICollection<Train> Trains { get; set; } = [];
}
