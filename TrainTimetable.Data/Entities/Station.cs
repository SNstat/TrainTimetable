using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Station : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsNode { get; set; } = false;

    [Required]
    public int CountryID { get; set; }
    [Required]
    public virtual Country Country { get; set; }
}
