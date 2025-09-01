using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controller deste�i
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Card Management API",
        Version = "v1"
    });
});

var app = builder.Build();

// Swagger UI a�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Management API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Controller�lar� map et
app.MapControllers();

app.Run();
