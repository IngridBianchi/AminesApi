using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary.Data;
using Oracle.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBlob, BlobStorage>(); 
builder.Services.AddLogging(logging => logging.AddConsole());

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

async Task<List<Adult>> GetAdults(DataContext context) => await context.Adults.ToListAsync();
app.MapGet("/Adults", async (DataContext context) => await GetAdults(context))
    .WithName("GetAdults")
    .WithOpenApi();

app.MapGet("/Adults/{id}", async (DataContext context, int id) => await context.Adults.FindAsync(id))
    .WithName("GetAdultById")
    .WithOpenApi();

app.MapPost("/Add/Member", async (DataContext context, IBlob blob, [FromForm] string name, [FromForm] string lastname, [FromForm] int birthyear, [FromForm] IFormFile? file) =>
{
    try
    {
        string? imageUrl = null;
        string? fileName = null;

        if (file != null)
        {
            // Usa la extensión original del archivo
            var extension = Path.GetExtension(file.FileName).ToLower() ?? ".jpg";
            fileName = $"members/{name.ToLower()}{lastname.ToLower()}_{DateTime.UtcNow.Ticks}{extension}";
            await blob.Upload(file, fileName);
            imageUrl = $"https://objectstorage.{builder.Configuration["OCI:Region"]}.oraclecloud.com/n/{builder.Configuration["OCI:Namespace"]}/b/{builder.Configuration["OCI:BucketName"]}/o/{fileName}";
        }

        if (birthyear <= 17)
        {
            var item = new Child
            {
                Name = name,
                Lastname = lastname,
                BirthYear = birthyear,
                ImageURL = imageUrl // Corrige a ImageURL
            };
            context.Children.Add(item);
        }
        else
        {
            var item = new Adult
            {
                Name = name,
                Lastname = lastname,
                BirthYear = birthyear,
                ImageURL = imageUrl // Corrige a ImageURL
            };
            context.Adults.Add(item);
        }

        await context.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error adding member: {ex.Message}");
    }
})

.WithName("AddMember")
.WithOpenApi()
.DisableAntiforgery(); 

app.Run();