using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Country
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}
