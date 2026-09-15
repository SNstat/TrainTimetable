using TrainTimetable.Data.Entities;

namespace TrainTimetable.Business.Services;

public interface IPricingService
{
    decimal CalculatePrice(TimeSpan timeSpan);
    decimal ApplyDiscount(decimal price, UserType userType);
    string DoubleDigit(decimal price);
}

public class PricingService : IPricingService
{
    public decimal CalculatePrice(TimeSpan timeSpan) =>
        (decimal)(1 + timeSpan.TotalHours * 1.8);

    public decimal ApplyDiscount(decimal price, UserType userType) =>
        userType switch
        {
            UserType.Regular => price,
            UserType.Student => price * (decimal)0.75,
            UserType.Senior => price * (decimal)0.55,
            _ => price
        };

    public string DoubleDigit(decimal price) =>
        price.ToString("F2");
}
