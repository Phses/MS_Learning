using CommandService.AsyncDataService;
using CommandService.Data;
using CommandService.EventProcessing;
using CommandService.SyncDataService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine(builder.Configuration.GetValue<string>("RabbitMQHost"));

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

builder.Services.AddSingleton<IEventProcessing, EventProcessing>();

builder.Services.AddScoped<ICommandRepo, CommandRepo>();

builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

builder.Services.AddHostedService<MessageBusSubscriber>();

//teste
var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();

