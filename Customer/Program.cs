using Customer.Data;
using Customer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IVehicleService, VehicleService>();

var vehicleServiceUrl = builder.Configuration.GetSection("VehicleServiceUrl").Value;
if (!string.IsNullOrEmpty(vehicleServiceUrl))
{
    builder.Services.AddHttpClient<IVehicleService, VehicleService>(client =>
    {
        client.BaseAddress = new Uri(vehicleServiceUrl);
    });
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();

