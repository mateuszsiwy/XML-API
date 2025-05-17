using System.Net;
using BazyProjekt.Interfaces.Repositories;
using BazyProjekt.Interfaces.Services;
using BazyProjekt.Repositories;
using BazyProjekt.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string xPath = "/book/title";
string encodedXPath = WebUtility.UrlEncode(xPath);
Console.WriteLine(encodedXPath); // Wynik: %2Fbook%2Ftitle
builder.Services.AddScoped<IXMLRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string is not configured TRALALALA.");
    }
    return new XMLRepository(connectionString);
});
builder.Services.AddScoped<IXMLService, XMLService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.MapControllers();
app.Run();