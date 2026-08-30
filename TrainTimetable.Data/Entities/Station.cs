using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

public class Station : IDbSet
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int? BaseStationID { get; set; }
    [ForeignKey(nameof(BaseStationID))]
    public virtual Station? BaseStation { get; set; }

    public int CountryID { get; set; }
    [ForeignKey(nameof(CountryID))]
    public virtual Country? Country { get; set; }
}
