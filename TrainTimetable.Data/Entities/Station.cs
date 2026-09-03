using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Station : BaseEntity
{
    [Required, StringLength(Constants.NAME_LENGTH)]
    public string Name { get; set; } = string.Empty;

    public int? BaseStationID { get; set; }
    public virtual Station? BaseStation { get; set; }

    [Required]
    public int CountryID { get; set; }
    [Required]
    public virtual Country Country { get; set; }
}
