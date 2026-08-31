using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class TrainManufacturer : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
    public string Name { get; set; } = String.Empty;

    public virtual ICollection<Train> Trains { get; set; } = [];
}
