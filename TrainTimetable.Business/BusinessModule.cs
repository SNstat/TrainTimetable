using Microsoft.Extensions.DependencyInjection;
using TrainTimetable.Business.Models;
using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business;

public static class BusinessModule
{
    public static IServiceCollection AddBusinessModule(this IServiceCollection services) {
        services.AddScoped<IBaseRepository<Train>, BaseRepository<Train>>();
        services.AddScoped<IBaseRepository<TrainManufacturer>, BaseRepository<TrainManufacturer>>();
        services.AddScoped<IBaseRepository<LineSchedule>, BaseRepository<LineSchedule>>();
        services.AddScoped<IBaseRepository<Station>, BaseRepository<Station>>();
        services.AddScoped<IBaseRepository<Ticket>, BaseRepository<Ticket>>();
        services.AddScoped<IBaseRepository<TicketSchedule>, BaseRepository<TicketSchedule>>();

        services.AddScoped<ITrainService, TrainService>();
        services.AddScoped<ITimetableService, TimetableService>();
        services.AddScoped<IStationService, StationService>();
        services.AddScoped<ITicketService, TicketService>();

        services.AddScoped<IEntityNavigationService<TimetableItem>, EntityNavigationService<TimetableItem>>();

        services.AddTransient<IPaymentService, PaymentService>();

        return services;
    }
}
