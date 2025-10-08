using GetAdults.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddScoped<IAdultService, AdultService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
