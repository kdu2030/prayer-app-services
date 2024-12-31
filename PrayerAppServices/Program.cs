using PrayerAppServices.Configuration;
using PrayerAppServices.Error;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IHostEnvironment environment = builder.Environment;

string appSettingsPath = environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.Production.json";
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(appSettingsPath)
    .AddEnvironmentVariables()
    .Build();

builder.Configuration.AddConfiguration(configuration);

Console.WriteLine("CONNECTION ENVIRONMENT VAR:");
Console.WriteLine(Environment.GetEnvironmentVariable("CONNECTION_STRINGS__DEFAULT_CONNECTION"));

Console.WriteLine("CONNECTION STRING: ");
Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = ErrorHandler.HandleDataValidationErrors);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext(configuration);
builder.Services.AddJwtConfiguration(configuration);

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(app => app.Run(context => ErrorHandler.HandleExceptionAsync(context)));

app.UseAuthorization();

app.MapControllers();

app.Run();
