using Application;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;
using Web.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation()
    .AddWebApi(builder.Host);

Log.Logger.Information("Application is building...");

try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app
        .UsePersistence(app.Environment)
        .UseWebApi(app.Environment);

    app.UsePresentation();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseInfrastructure();

    app.MapControllers();

    Log.Logger.Information("Application is running...");

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Logger.Error(exception, "Application failed to start...");
    Console.WriteLine(exception.ToString());
}
finally
{
    await Log.CloseAndFlushAsync();
}

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}