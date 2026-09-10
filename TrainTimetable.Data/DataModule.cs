using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
}
