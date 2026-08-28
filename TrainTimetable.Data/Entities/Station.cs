using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Station
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public virtual Station? BaseStation { get; set; }

    [Required]
    public virtual Country? Country { get; set; }
}
