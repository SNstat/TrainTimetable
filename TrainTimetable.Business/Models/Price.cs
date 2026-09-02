namespace TrainTimetable.Business.Models;

public static class PriceExtension
{
    // Uses business rule formula: t => number of hours, Price = 1 + 0.5t 
    public static decimal ToPrice(this TimeSpan timeSpan) => 
        (decimal)(1 + timeSpan.TotalHours * 0.5);

    public static string ToDoubleDigit(this decimal price) =>
        price.ToString("F2");
}
