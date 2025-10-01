// ControlGastos.API/Program.cs
using ControlGastos.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Control de Gastos API", Version = "v1" });
});

// Add Infrastructure services
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Control de Gastos API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Usando RunAsync en lugar de Run para mejor manejo de recursos asíncronos
await app.RunAsync();