using BusinessLogic;
using Data.Models;
using Data;
using WebClient;
using PowderBot;

var builder = WebApplication.CreateBuilder(args);

// Register services
RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure logging to files
var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    // Add controllers
    builder.Services.AddControllers().AddNewtonsoftJson();

    // Use ApplicationInsights
    services.AddApplicationInsightsTelemetry(configuration);

    // Register configurations
    services.Configure<StorageConfiguration>(configuration.GetSection("Storage"));
    services.Configure<TelegramConfiguration>(configuration.GetSection("Telegram"));
    services.Configure<SchedulerConfiguration>(configuration.GetSection("Scheduler"));

    // Solution services registration
    services.AddScoped<HttpClient, HttpClient>();
    services.AddScoped<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
    services.AddScoped<IGenericRepository<SubscriptionModel>, GenericRepository<SubscriptionModel>>();
    services.AddScoped<UserRepository, UserRepository>();
    services.AddScoped<SubscriptionRepository, SubscriptionRepository>();
    services.AddScoped<CommandFactory, CommandFactory>();
    services.AddScoped<IMessanger, TelegramClient>();
    services.AddScoped<ISnowForecastClient, SnowForecastClient>();
    services.AddScoped<SnowfallChecker, SnowfallChecker>();
}