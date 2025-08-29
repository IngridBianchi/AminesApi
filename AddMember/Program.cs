using AddMember.Data;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración desde appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Registrar IServiceBus como servicio transitorio
builder.Services.AddTransient<IServiceBus, ServiceBus>();

// Agregar servicios de anti-forgery (opcional, pero necesario si usas UseAntiforgery)
builder.Services.AddAntiforgery();

// Agregar Swagger para explorar y probar la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar middleware de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agregar middleware de anti-forgery (puedes dejarlo o quitarlo si deshabilitas para el endpoint)
app.UseAntiforgery();

// Definir el endpoint POST /Add/Member con anti-forgery deshabilitado
app.MapPost("Add/Member", async (IFormFile file, IServiceBus serviceBus, string name, string lastname, int birthyear) =>
{
    // Verificar que los parámetros no estén vacíos
    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastname) || birthyear <= 0)
    {
        return Results.BadRequest("Los parámetros name, lastname y birthyear son requeridos y válidos.");
    }

    // Enviar el mensaje a RabbitMQ usando el servicio ServiceBus
    await serviceBus.SendMessageAsync(name, lastname, birthyear.ToString());

    // Respuesta exitosa
    return Results.Ok($"Mensaje enviado para {name} {lastname} con birthyear {birthyear}");
})
.DisableAntiforgery() // Deshabilita anti-forgery solo para este endpoint
.WithName("AddMember");

app.Run();