using System;
using System.Diagnostics;
using AniNexus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

try
{
    var appBuilder = new AniNexusApplicationBuilder()
        .ConfigureGlobalLogger();

    var logger = appBuilder.GetSerilogLogger<Program>();

    logger.Information("Building the application host.");
    var app = appBuilder.Build(args);
    if (app is null)
    {
        logger.Fatal("Unable to build the application host.");
        return -1;
    }
    logger.Information("Application host built successfully.");

    logger.Information("Configuring the application host.");
    app.MapControllers();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
    }

    app
        .UseStaticFiles()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    logger.Information("Application host configured successfully.");

    try
    {
        logger.Information("Running the application host.");
        await app.RunAsync();
        logger.Information("The application host has stopped successfully.");
        return 0;
    }
    catch (Exception e)
    {
        logger.Fatal(e, "The application host terminated unexpectedly.");

        if (Debugger.IsAttached && Environment.UserInteractive)
        {
            Console.WriteLine(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey(true);
        }

        return e.HResult;
    }
}
finally
{
    await Log.CloseAndFlushAsync();
}
