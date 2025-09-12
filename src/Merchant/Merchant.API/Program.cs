using CardManagement.Data.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<CardManagementContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CardManagement.Data")
    )
);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000", // React dev server
                "https://merchant-frontend-gwfbb5gpe6fdgndr.francecentral-01.azurewebsites.net", // deployed frontend
                "https://merchantfrontend-bufagrdgbvcpbke6.francecentral-01.azurewebsites.net" // your new frontend
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // required if using cookies/auth
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger (accessible at /swagger)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Management API v1");
    c.RoutePrefix = "swagger"; // ensures https://your-backend.azurewebsites.net/swagger works
});

// HTTPS redirection
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// Apply CORS policy BEFORE Authorization
app.UseCors("AllowFrontend");

// Authorization middleware (if using)
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
