namespace TrainTimetable.Business.Models;

public static class PriceExtension
{
    // Uses business rule formula: t => number of hours, Price = 1 + 1.8t 
    public static decimal ToPrice(this TimeSpan timeSpan) => 
        (decimal)(1 + timeSpan.TotalHours * 1.8);

    public static string ToDoubleDigit(this decimal price) =>
        price.ToString("F2");
}
