using Autofac.Extensions.DependencyInjection;
using Autofac;
using Serilog;
using Serilog.Events;
using Stackoverflow.API;
using Stackoverflow.Application;
using Stackoverflow.Infrastructure;
using Stackoverflow.Infrastructure.Extensions;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Stackoverflow.Infrastructure.Requirements;
using Stackoverflow.API.RequestHandlers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DotNetEnv.Configuration;
using Autofac.Core;

string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "api.env");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddDotNetEnv(envFilePath) // Specify the full path to the .env file) // Simply add the DotNetEnv configuration source!
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();

try
{
    Log.Information("API Application Bulding...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration));

    var connectionStringConfig = builder.Configuration.GetConnectionString("DefaultAPIConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var connectionString = System.Environment.GetEnvironmentVariable("DefaultAPIConnection") ?? connectionStringConfig;
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;
    //Get connectionstrings in log
    Log.Information("Connection String:" + connectionString);

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString,
            migrationAssembly));
        containerBuilder.RegisterModule(new ApiModule());
    });

    // Add services to the container.

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddIdentity();
    builder.Services.AddJwtAuthentication(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"],
        builder.Configuration["Jwt:Audience"]);

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("PostViewRequirementPolicy", policy =>
        {
            policy.AuthenticationSchemes.Clear();
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new PostViewRequirement());
        });
    });


    //builder.Services.AddCors(options =>
    //{
    //    options.AddPolicy("AllowSites",
    //        builder =>
    //        {
    //            //builder.WithOrigins("https://localhost:7000", "https://localhost:5041", "http://localhost:7000", "http://localhost:5001", "http://localhost:5041", "http://localhost")
    //            //builder.WithOrigins("https://localhost:7000", "http://localhost:8080", "https://localhost:8080", "http://localhost", "http://localhost:6969", "http://localhost:5153", "http://localhost:80")
    //            //builder.WithOrigins("http://localhost:8080", "https://localhost:44361", "localhost:6969", "https://localhost:8080/", "http://localhost:8080/", "http://localhost:8080","https://localhost:8080", "http://localhost", "http://localhost:6969", "http://localhost:5153", "http://localhost:80", "https://localhost:7000", "http://localhost:7000", "http://localhost:5041", "https://localhost:5041")
    //            builder.AllowAnyOrigin()
    //               .AllowAnyMethod()
    //               .AllowAnyHeader();
    //        });
    //});


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowWebApp",
            builder =>
            {
                builder
                  .WithOrigins("http://localhost:5759", "http://localhost:8080", "https://localhost:44361", "https://stackoverflow-clone.azurewebsites.net/") // Replace with the URL of your web app
                  .AllowAnyMethod()
                  .AllowAnyHeader();
            });
    });


    builder.Services.AddSingleton<IAuthorizationHandler, PostViewRequirementHandler>();
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

    var app = builder.Build();

    Log.Information("Application Starting...");

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseCors("AllowWebApp");

    app.UseAuthorization();

    app.MapControllers();
    Log.Information("API Application Running...");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
