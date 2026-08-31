using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public interface IBaseEntity
{
    int ID { get; set; }
}

public class BaseEntity : IBaseEntity
{
    [Key]
    public int ID { get; set; }
}
