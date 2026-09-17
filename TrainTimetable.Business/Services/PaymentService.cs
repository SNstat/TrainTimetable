using TrainTimetable.Data.Entities;

namespace TrainTimetable.Business.Services;

public interface IPaymentService
{
    IReadOnlyDictionary<UserType, decimal> Discounts { get; }
    decimal CalculatePrice(TimeSpan timeSpan, int seatCount);
    decimal CalculatePriceWithDiscount(TimeSpan timeSpan, int seatCount, UserType userType);
    string DoubleDigit(decimal price);
    Task<bool> PayAsync(bool valid);
}

public class PaymentService : IPaymentService
{
    private const decimal BASEFEE = 1m;
    private const decimal PERHOUR = 1.8m;

    public IReadOnlyDictionary<UserType, decimal> Discounts { get; } = new Dictionary<UserType, decimal>
    {
        { UserType.Regular, 0m },
        { UserType.Student, 0.25m },
        { UserType.Senior, 0.40m }
    };

    public decimal CalculatePrice(TimeSpan timeSpan, int seatCount) =>
        (BASEFEE + ((decimal)timeSpan.TotalHours * PERHOUR)) * seatCount;

    public decimal CalculatePriceWithDiscount(TimeSpan timeSpan, int seatCount, UserType userType)
    {
        var discount = 1m - Discounts.GetValueOrDefault(userType, 0m);
        return CalculatePrice(timeSpan, seatCount) * discount;
    }

    public string DoubleDigit(decimal price) =>
        price.ToString("F2");

    public async Task<bool> PayAsync(bool valid)
    {
        await Task.Delay(3000);
        return valid;
    }
}
