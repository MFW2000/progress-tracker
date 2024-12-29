using MFW.ProgressTracker.Components;
using MFW.ProgressTracker.Services;
using MFW.ProgressTracker.Services.Interfaces;

namespace MFW.ProgressTracker;

/// <summary>
/// Represents the primary entry point for configuring and running the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The entry point of the application and its configurations.
    /// </summary>
    /// <param name="args">An array of command-line arguments passed to the application.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        // Add injectable services.
        builder.Services.AddScoped<ITrackerService, TrackerService>();

        builder.Services.AddSingleton<INotificationService, NotificationService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // Run the application with the provided configuration.
        app.Run();
    }
}
