using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Country : BaseEntity
{
    [Required, StringLength(Constants.NAMELENGTH)]
    public string Name { get; set; } = string.Empty;
}
