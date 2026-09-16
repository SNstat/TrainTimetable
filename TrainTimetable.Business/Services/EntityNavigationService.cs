namespace TrainTimetable.Business.Services;

public interface IEntityNavigationService<T> where T : class
{
    T? Entity { get; set; }
}

public class EntityNavigationService<T> : IEntityNavigationService<T> where T : class
{
    public T? Entity { get; set; }
}