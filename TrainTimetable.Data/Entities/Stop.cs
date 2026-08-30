using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Data.Entities;

[Index(nameof(LineID), nameof(Order), IsUnique = true, Name = "1_UniqueOrderPerLine")]
[Index(nameof(LineID), nameof(StationID), IsUnique = true, Name = "2_UniqueStationsPerLine")]
public class Stop : IDbSet
{
    [Key]
    public int ID { get; set; }
    
    public int StationID { get; set; }
    [ForeignKey(nameof(StationID))]
    public virtual Station? Station { get; set; }

    public int LineID {  get; set; }
    [ForeignKey(nameof(LineID))]
    public virtual Line? Line { get; set; }

    [Required]
    public int Order { get; set; }

    public TimeOnly? ArrivalTime { get; set; }

    public TimeOnly? DepartureTime { get; set; }
}
