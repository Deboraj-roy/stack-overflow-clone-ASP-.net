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
using Microsoft.AspNetCore.Builder;
using Stackoverflow.Infrastructure.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DotNetEnv.Configuration;
using DotNetEnv; 
using System.IO;

//DotNetEnv.Env.Load();
//DotNetEnv.Env.TraversePath().Load();


string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "web.env");


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddDotNetEnv(envFilePath) // Specify the full path to the .env file) // Simply add the DotNetEnv configuration source!
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();

// Access environment variables
string apiKey = DotNetEnv.Env.GetString("AWS_ACCESS_KEY_ID");
string dbHost = DotNetEnv.Env.GetString("AWS_SECRET_ACCESS_KEY", "localhost"); // Default value if not found

// Use the environment variables
Console.WriteLine($"API Key: {apiKey}");
Console.WriteLine($"DB Host: {dbHost}");

try
{
    Log.Information("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultStackoverflowConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    //Log.Information("Connection String:" + connectionString);

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
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCookieAuthentication();
    builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
     
    //googleReCaptcha
    builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("SupperAdmin", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(UserRoles.Admin);
            policy.RequireRole(UserRoles.Elite);
        });

        options.AddPolicy("PostCreatePolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("CreatePost", "true");
        });

        options.AddPolicy("PostUpdatePolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("UpdatePost", "true");
        });

        options.AddPolicy("PostViewPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("ViewPost", "true");
        });

        options.AddPolicy("PostViewRequirementPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new PostViewRequirement());
        });

    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddSingleton<IAuthorizationHandler, PostViewRequirementHandler>();
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));


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

    app.UseHttpsRedirection()
        .UseStaticFiles()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseSession();

    app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();

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
