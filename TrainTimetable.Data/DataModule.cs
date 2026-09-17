using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrainTimetable.Data.Seeds;

namespace TrainTimetable.Data;

public static class DataModule
{
    public static IServiceCollection AddDataModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static async Task MigrateDataAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var dbContextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();

        await dbContext.Database.MigrateAsync();
    }

    public static async Task MigrateIdentityDataAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var services = serviceScope.ServiceProvider;
        await IdentityDataSeeder.SeedRoleAndUSers(services);
    }
}
