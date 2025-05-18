using Microsoft.EntityFrameworkCore;
using ProyectoCentinela.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Validación de la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
}

// Configuración del DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// Configuración de CORS para permitir Unity
builder.Services.AddCors(options =>
{
    options.AddPolicy("UnityPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Swagger UI para documentación, solo en desarrollo
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Proyecto Centinela API",
            Version = "v1",
            Description = "API para Proyecto Centinela, gestionando usuarios, roles y leaderboard."
        });
    });
}

// Registro de controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Middleware y configuración
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto Centinela API v1");
        c.RoutePrefix = "swagger"; // Hace que Swagger sea la página principal
    });
}

// Redirección a HTTPS (puedes activarlo si estás en producción)
// app.UseHttpsRedirection();

app.UseCors("UnityPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();