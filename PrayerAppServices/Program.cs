WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IHostEnvironment environment = builder.Environment;

string appSettingsPath = environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.Production.json";
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile(appSettingsPath)
    .Build();
builder.Configuration.AddConfiguration(configuration);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
