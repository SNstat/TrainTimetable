using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrainTimetable.Data.Entities;

public enum UserType
{
    Regular,
    Student,
    Senior,
    Employee
}

public class ApplicationUser : IdentityUser
{
    [Required]
    public UserType UserType { get; set; } = UserType.Regular;
}
