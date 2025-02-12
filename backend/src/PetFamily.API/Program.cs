using System.Reflection;
using PetFamily.API;
using PetFamily.API.Common;
using PetFamily.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();