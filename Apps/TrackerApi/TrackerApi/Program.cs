using MediatR;
using System.Reflection;
using TrackerApi.Features.ObservedObject.Create.Services;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins");
        policy.WithOrigins(allowedOrigins.Split(";"))
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddSignalR();
builder.Services.Configure<MongoConfiguration>(
    builder.Configuration.GetSection(nameof(MongoConfiguration)));
builder.Services.AddTransient<IDataRepository, DataRepository>();
builder.Services.AddTransient<IHistoricalViolationService, HistoricalViolationService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapHub<CoordinatesObservedObjectHub>("/observedObjectCoordinates");
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

//app.Run();                                // global https://sledz.fokser.com ; for debugging: http://localhost:5142/
app.Run("http://192.168.1.163:5200");       // local, can be moved to launchSettings.json
