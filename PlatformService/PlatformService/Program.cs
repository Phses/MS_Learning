using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;
using PlatformService.SyncDataService.http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

Console.WriteLine("---> testando modificacoes do build ");

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<IPlatformDataClient, HttpPlatformDataClient>();

builder.Services.AddGrpc();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("---> Use inMemory Database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseInMemoryDatabase("InMemory");
    });
}
else
{
    Console.WriteLine("---> Use sqlserver Database");
    Console.WriteLine(builder.Configuration.GetConnectionString("PlatformConn"));
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn"));
    });
}



Console.WriteLine(builder.Configuration.GetValue<string>("CommandService"));

builder.Services.AddCors(x =>
            {
              x.AddPolicy("PlatformCors", config =>
              {
                config.AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetIsOriginAllowed(x => true)
                  .AllowCredentials();
              });
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<GrpcPlatformService>();

    endpoints.MapGet("/protos/platform.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platform.proto"));
    });
});

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
