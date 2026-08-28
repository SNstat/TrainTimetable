using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public class Stop
{
    [Key]
    public int ID { get; set; }

    public int StationID { get; set; }
    [Required]
    public virtual Station? Station { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }

    [Required]
    public DateTime DepartureTime { get; set; }

}
