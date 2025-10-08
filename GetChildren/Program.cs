using GetChildren.Services;
using GetChildren.Models;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
