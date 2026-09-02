using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Stop : BaseEntity
{
    [Required]
    public int StationID { get; set; }
    public required virtual Station Station { get; set; }

    [Required]
    public int LineID {  get; set; }
    public required virtual Line Line { get; set; }

    [Required]
    public int Order { get; set; }

    public TimeSpan? ArrivalOffset{ get; set; }

    public TimeSpan? DepartureOffset { get; set; }
}
