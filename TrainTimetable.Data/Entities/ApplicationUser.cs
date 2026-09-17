using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTimetable.Data.Entities;

public enum UserType
{
    Regular,
    Student,
    Senior
}

public class ApplicationUser : IdentityUser
{
    [Required]
    public UserType UserType { get; set; } = UserType.Regular;

    [ForeignKey("UserId")]
    public virtual ICollection<Ticket> Tickets { get; set; }
}
