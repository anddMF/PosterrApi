using Posterr.API.Entities;
using Posterr.API.Infrastructure.Factories;
using Posterr.API.Interfaces;
using Posterr.API.Middlewares;
using Posterr.API.Services;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json");
        var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton(appSettings);

        builder.Services.AddScoped<IDBCommunicationFactory, DBCommunicationFactory>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}