using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Seeds;

internal sealed record ApplicationUserDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public bool LockoutEnabled { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
   
    public IEnumerable<string> Roles { get; set; } = [];
}

internal sealed class IdentityDataSeeder
{
    internal static async Task SeedRoleAndUSers(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var appUserDtos = await LoadUsersFromJson();

        foreach (var user in appUserDtos)
        {
            if (user != null && !string.IsNullOrWhiteSpace(user.Email) && !string.IsNullOrWhiteSpace(user.Password) &&
                await userManager.FindByEmailAsync(user.Email) == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = user.Email,
                    NormalizedUserName = user.Email.ToUpper(),
                    Email = user.Email,
                    NormalizedEmail = user.Email!.ToUpper(),
                    EmailConfirmed = true,
                    LockoutEnabled = user.LockoutEnabled
                };

                var result = await userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded && !user.Roles.IsNullOrEmpty())
                {
                    foreach (var role in user.Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole { Name = role });
                        }

                        await userManager.AddToRoleAsync(newUser, role);
                    }
                }
            }
        }
    }

    private static async Task<IEnumerable<ApplicationUserDto>> LoadUsersFromJson()
    {
        string filePath = String.Concat("../TrainTimetable.Data/Data/ApplicationUsers.json");

        await using var stream = File.OpenRead(filePath);

        return await JsonSerializer.DeserializeAsync<List<ApplicationUserDto>>(stream) ?? [];
    }
}
