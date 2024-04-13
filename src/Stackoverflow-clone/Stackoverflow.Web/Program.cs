using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application;
using Stackoverflow.Infrastructure;
using Stackoverflow.Web;
using System.Reflection;
using Serilog;
using Serilog.Events;
using Stackoverflow.Infrastructure.Extensions;
using Stackoverflow.Infrastructure.Email;
using Stackoverflow.Infrastructure.Membership;
using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/web-log-.log", rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();
try
{
    Log.Information("Application Starting...");

    var connectionString = builder.Configuration.GetConnectionString("DefaultStackoverflowConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    Log.Information("Connection String:" + connectionString);

    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new InfrastructureModule(connectionString,
            migrationAssembly));
        containerBuilder.RegisterModule(new WebModule());
    });


    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,
        (m) => m.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCookieAuthentication();
    builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
    //googleReCaptcha
    builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

    builder.Services.AddAuthorization( options =>
    {
        options.AddPolicy("SupperAdmin", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(UserRoles.Admin);
            policy.RequireRole(UserRoles.Elite);
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{area:exists}/{controller=Post}/{action=Index}/{id?}",
            defaults: new { area = "user" }
        );
        endpoints.MapRazorPages();
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    Log.CloseAndFlush();
}
