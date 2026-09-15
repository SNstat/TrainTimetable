using TrainTimetable.Data.Entities;

namespace TrainTimetable.Business.Services;

public interface IPricingService
{
    Task<decimal> CalculatePrice(TimeSpan timeSpan);
    Task<decimal> ApplyDiscount(decimal price, UserType userType);
    Task<string> DoubleDigit(decimal price);
}

public class PricingService : IPricingService
{
    public async Task<decimal> CalculatePrice(TimeSpan timeSpan) =>
        (decimal)(1 + timeSpan.TotalHours * 1.8);

    public async Task<decimal> ApplyDiscount(decimal price, UserType userType) =>
        userType switch
        {
            UserType.Regular => price,
            UserType.Student => price * (decimal)0.75,
            UserType.Senior => price * (decimal)0.55,
            _ => price
        };

    public async Task<string> DoubleDigit(decimal price) =>
        price.ToString("F2");
}
