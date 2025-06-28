using System.Text.Json.Serialization;
using MyNorthwind.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Prevent circular reference issues by using ReferenceHandler.Preserve
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<NorthwindContext>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseSwaggerUI(e => e.SwaggerEndpoint("/openapi/v1.json", "Swagger Northwind"));
app.MapControllers();
app.Run();