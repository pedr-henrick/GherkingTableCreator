using GherkinCreator.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/v1/createGherkin", ([FromBody] JsonElement jsonFromTransform) =>
{
    try
    {
        return GherkinCreatorService.CreateGherkin(jsonFromTransform);
    }
    catch (Exception)
    {
        throw;
    }
})
.WithName("CreateGherking")
.WithOpenApi();

app.Run();
