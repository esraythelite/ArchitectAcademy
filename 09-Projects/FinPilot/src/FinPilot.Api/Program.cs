using FinPilot.Application.Onboarding.StartOnboarding;
using FinPilot.Application.Onboarding.Persistence;
using FinPilot.Application.Abstractions.Persistence;
using FinPilot.Infrastructure.Persistence;
using FinPilot.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using FinPilot.Api.ExceptionHandling;
using FinPilot.Application.Onboarding.GetOnboardingById;
using FinPilot.Application.Onboarding.VerifyIdentity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' not found.")  ;

builder.Services.AddDbContext<FinPilotDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IOnboardingRepository, OnboardingRepository>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<FinPilotDbContext>());
builder.Services.AddScoped<StartOnboardingHandler>();
builder.Services.AddScoped<GetOnboardingByIdHandler>();
builder.Services.AddScoped<VerifyIdentityHandler>();


var app = builder.Build();
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
