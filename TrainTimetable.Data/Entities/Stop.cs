using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public class Stop : BaseEntity
{ 
    public int StationID { get; set; }
    [ForeignKey(nameof(StationID))]
    public virtual Station? Station { get; set; }

    public int LineID {  get; set; }
    [ForeignKey(nameof(LineID))]
    public virtual Line? Line { get; set; }

    [Required]
    public int Order { get; set; }

    public TimeSpan? ArrivalOffset{ get; set; }

    public TimeSpan? DepartureOffset { get; set; }
}
