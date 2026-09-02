namespace TrainTimetable.Data.Models;

[Flags]
public enum DrivingDays
{
    NotActive = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64,

    WorkDays = Monday | Tuesday | Wednesday | Thursday | Friday,
    WeekEnd = Saturday | Sunday,
    Any = WorkDays | WeekEnd
};

public static class DrivingDaysExtension
{
    public static DrivingDays ToDrivingDays(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => DrivingDays.Monday,
            DayOfWeek.Tuesday => DrivingDays.Tuesday,
            DayOfWeek.Wednesday => DrivingDays.Wednesday,
            DayOfWeek.Thursday => DrivingDays.Thursday,
            DayOfWeek.Friday => DrivingDays.Friday,
            DayOfWeek.Saturday => DrivingDays.Saturday,
            DayOfWeek.Sunday => DrivingDays.Sunday,
            _ => throw new ArgumentException("Invalid argument!"),
        };
    }
}
