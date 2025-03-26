using BankAPI.API.Middlewares;
using BankAPI.Application;
using BankAPI.Infrastructure;
using BankAPI.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Definir fuso hor�rio globalmente
TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

builder.Services.AddSingleton(timeZone);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddBankContext(builder.Configuration);
builder.Services.AddResilientOperation();
builder.Services.AddUnitOfWork();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bank API",
        Version = "v1",
        Description = "API para gest�o de contas banc�rias.",
        Contact = new OpenApiContact
        {
            Name = "Paulo Aguiar Junior",
            Email = "paguiar.ti@gmail.com"            
        }
    });
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankContext>();

    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.Run();
