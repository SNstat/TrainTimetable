using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class Train : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int SeatCount { get; set; }

    [Required]
    public int TrainManufacturerID { get; set; }
    [Required]
    public virtual TrainManufacturer TrainManufacturer { get; set; } = new TrainManufacturer();

    public virtual ICollection<LineSchedule> LineSchedules { get; set; } = [];

    [NotMapped]
    public bool IsActive => LineSchedules.Count > 0;
}
