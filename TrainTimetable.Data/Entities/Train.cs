using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class Train : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
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
