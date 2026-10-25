using MiniBank.Api.Middleware;
using MiniBank.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseExceptionHandler();
app.UseCorrelationId();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
