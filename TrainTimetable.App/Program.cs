using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TrainTimetable.App.Identity;
using TrainTimetable.App.Identity.Services;
using TrainTimetable.Business;
using TrainTimetable.Data;

namespace TrainTimetable.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddIdentityModule();
        builder.Services.AddBusinessModule();
        builder.Services.AddDataModule(builder.Configuration.GetConnectionString("DefaultConnection")!);
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

        app.MapRazorComponents<Components.Core.App>()
            .AddAdditionalAssemblies(typeof(IdentityModule).Assembly)
            .AddInteractiveServerRenderMode();

        using var serviceScope = app.Services.CreateScope();
        var dbContextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();       
        var dbContext = await dbContextFactory.CreateDbContextAsync();

        app.MapAdditionalIdentityEndpoints();

        await dbContext.Database.MigrateAsync();

        app.Run();
    }
}
