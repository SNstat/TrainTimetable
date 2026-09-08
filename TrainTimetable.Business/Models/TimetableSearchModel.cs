using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Business.Models;

public enum TripType
{
    OneWay,
    Return
}

public record TimetableSearchModel
{
    [Required]
    public StationItem? DepartureStationItem { get; set; }

    [Required]
    public StationItem? ArrivalStationItem { get; set; }

    [Required]
    [DateBetweenTodayAndAYear]
    public DateTime? SearchDate { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "The seat count must be between 1 and 100.")]
    public int SeatCount { get; set; } = 1;

    [Required]
    public TripType TripType { get; set; } = TripType.Return;
}

public class DateBetweenTodayAndAYearAttribute : ValidationAttribute
{
    public DateBetweenTodayAndAYearAttribute()
    {
        ErrorMessage = "The date must be between today and a year later.";
    }   

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            var dateUnderValidation = DateOnly.FromDateTime(dateTime);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var yearLater = today.AddYears(1);

            if (dateUnderValidation >= today && dateUnderValidation <= yearLater)
            {
                return ValidationResult.Success;
            }
        }
        
        return new ValidationResult(ErrorMessage);
    }
}
