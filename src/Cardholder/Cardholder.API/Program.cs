using Cardholder.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CardHolderContext>(dbContextOptions 
        => dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();


app.UseSwagger();                 
app.UseSwaggerUI();               

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();