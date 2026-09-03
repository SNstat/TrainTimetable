using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TrainTimetable.Business.Services;
using TrainTimetable.Data;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddScoped<IBaseRepository<Train>, BaseRepository<Train>>();
        builder.Services.AddScoped<ITrainService, TrainService>();
        builder.Services.AddScoped<IBaseRepository<Line>, BaseRepository<Line>>();
        builder.Services.AddScoped<IBaseRepository<LineSchedule>, BaseRepository<LineSchedule>>();
        builder.Services.AddScoped<ILineScheduleService, LineScheduleService>();

        builder.Services.AddMudServices();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();

        using var serviceScope = app.Services.CreateScope();
        var dbContextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();       
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        await dbContext.Database.MigrateAsync();

        app.Run();
    }
}
