using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Business.Models;

public enum TripType
{
    OneWay,
    Return
}

public record TimetableSearchModel
{
    [RequiredStation]
    [UniqueStations]
    public KeyValuePair<int, string> DepartureStation { get; set; }

    [RequiredStation]
    [UniqueStations]
    public KeyValuePair<int, string> ArrivalStation { get; set; }

    [Required]
    [DateBetweenTodayAndAYear]
    public DateTime? SearchDate { get; set; } = DateTime.Now;

    [Required]
    [Range(1, 100, ErrorMessage = "The seat count must be between 1 and 100.")]
    public int SeatCount { get; set; } = 1;

    [Required]
    public TripType TripType { get; set; }
}

public class RequiredStationAttribute : ValidationAttribute
{
    public RequiredStationAttribute()
    {
        ErrorMessage = "The station is required.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is KeyValuePair<int, string> station)
        {
            if (station.Key == 0 || string.IsNullOrWhiteSpace(station.Value))
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

public class DateBetweenTodayAndAYearAttribute : ValidationAttribute
{
    public DateBetweenTodayAndAYearAttribute()
    {
        ErrorMessage = "The date must be between today and a year later.";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            var dateUnderValidation = DateOnly.FromDateTime(dateTime);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var yearLater = today.AddYears(1);

            if (dateUnderValidation >= today && dateUnderValidation <= yearLater)
                return ValidationResult.Success!;
        }

        return new ValidationResult(ErrorMessage);
    }
}

public class UniqueStationsAttribute : ValidationAttribute
{
    public UniqueStationsAttribute()
    {
        ErrorMessage = "The departure and arrival stations must be different.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = (TimetableSearchModel)validationContext.ObjectInstance;

        if (model.DepartureStation.Key == 0 || model.ArrivalStation.Key == 0)
            return ValidationResult.Success;

        if (model.DepartureStation.Key == model.ArrivalStation.Key)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}