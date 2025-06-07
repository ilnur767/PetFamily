using PetFamily.Web;
using PetFamily.Web.Common;
using PetFamily.Web.Middlewares;
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
    await app.ApplyMigration();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

namespace PetFamily.Web
{
    public class Program;
}
