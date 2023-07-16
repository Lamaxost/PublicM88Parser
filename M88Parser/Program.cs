using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParsingWebTools;
using Serilog;

var builder = new ConfigurationBuilder();
BuildConfig(builder);


var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext();
loggerConfiguration.WriteTo.File("logs.txt")
.WriteTo.Console();
Log.Logger = loggerConfiguration.CreateLogger();
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<WebScrapper>();
        services.AddScoped<PagesFactory>();
        services.AddTransient<Starter>();
        services.AddTransient<M88Parser.Parser>();
    })
    .UseSerilog()
    .Build();


var starter = host.Services.GetRequiredService<Starter>();
await starter.Run();

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsetting.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
}

