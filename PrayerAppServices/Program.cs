using PrayerAppServices.Configuration;
using PrayerAppServices.Error;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IHostEnvironment environment = builder.Environment;

string appSettingsPath = environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.Production.json";
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(appSettingsPath)
    .AddEnvironmentVariables()
    .Build();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Configuration.AddConfiguration(configuration);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)
    .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = ErrorHandler.HandleDataValidationErrors);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext(configuration);
builder.Services.AddJwtConfiguration(configuration);

builder.Services.RegisterServices();

builder.Services.AddAutoMapper(typeof(Program));

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
