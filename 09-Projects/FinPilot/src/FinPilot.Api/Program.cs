using FinPilot.Application.Onboarding.StartOnboarding;

using FinPilot.Api.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<StartOnboardingHandler>();

var app = builder.Build();

app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
//app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program
{
}
