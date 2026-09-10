using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FinPilot.IntegrationTests;

public sealed class StartOnboardingEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StartOnboardingEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task StartOnboarding_WithValidRequest_ReturnsCreated()
    {
        var request = new
        {
            identityNumber = "11111111111",
            email = "test@example.com",
            phoneNumber = "+905551234567"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/onboardings",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task StartOnboarding_WithInvalidIdentityNumber_ReturnsBadRequest()
    {
        var request = new
        {
            identityNumber = "123",
            email = "test@example.com",
            phoneNumber = "+905551234567"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/onboardings",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task StartOnboarding_WithInvalidIdentityNumber_ReturnsBadRequest_WithProblemDetails()
    {
        var request = new
        {
            identityNumber = "123",
            email = "test@example.com",
            phoneNumber = "+905551234567"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/onboardings",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal("Bad Request", problemDetails.Title);
        Assert.Contains("Identity number must contain 11 characters", problemDetails.Detail);
    }
    
}