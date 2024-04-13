using Autofac.Extensions.DependencyInjection;
using Autofac;
using Serilog;
using Serilog.Events;
using Stackoverflow.API;
using Stackoverflow.Application;
using Stackoverflow.Infrastructure;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration));

    var connectionString = builder.Configuration.GetConnectionString("DefaultAPIConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

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
    //builder.Services.AddIdentity();
    //builder.Services.AddJwtAuthentication(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"],
    //    builder.Configuration["Jwt:Audience"]);

    //builder.Services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("PostViewRequirementPolicy", policy =>
    //    {
    //        policy.AuthenticationSchemes.Clear();
    //        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
    //        policy.RequireAuthenticatedUser();
    //        policy.Requirements.Add(new PostViewRequirement());
    //    });
    //});


    //builder.Services.AddSingleton<IAuthorizationHandler, PostViewRequirementHandler>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

    var app = builder.Build();

    Log.Information("Application Starting...");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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
