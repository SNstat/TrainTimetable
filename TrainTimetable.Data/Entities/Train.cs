using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Train : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int SeatCount { get; set; }

    public int TrainManufacturerID { get; set; }
    [ForeignKey(nameof(TrainManufacturerID))]
    public TrainManufacturer? TrainManufacturer { get; set; }

    public virtual ICollection<Line> Lines { get; set; } = [];

    [NotMapped]
    public bool IsActive => Lines.Count > 0;
}
