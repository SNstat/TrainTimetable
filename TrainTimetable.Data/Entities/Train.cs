using System.ComponentModel.DataAnnotations;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Train : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Manufacturer { get; set; } = string.Empty;

    [Required]
    public int SeatCount { get; set; }
}
