using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Country : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
    public string Name { get; set; } = string.Empty;
}
