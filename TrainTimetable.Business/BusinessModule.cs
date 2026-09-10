using Microsoft.Extensions.DependencyInjection;
using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business;

public static class BusinessModule
{
    public static IServiceCollection AddBusinessModule(this IServiceCollection services) {
        services.AddScoped<IBaseRepository<Train>, BaseRepository<Train>>();
        services.AddScoped<ITrainService, TrainService>();
        services.AddScoped<IBaseRepository<LineSchedule>, BaseRepository<LineSchedule>>();
        services.AddScoped<ITimetableService, TimetableService>();
        services.AddScoped<IBaseRepository<Station>, BaseRepository<Station>>();
        services.AddScoped<IStationService, StationService>();

        return services;
    }
}
