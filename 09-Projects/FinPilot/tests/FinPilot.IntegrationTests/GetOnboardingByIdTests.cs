using System.Net;
using System.Net.Http.Json;
using FinPilot.IntegrationTests.Infrastructure;
using FinPilot.Application.Onboarding.StartOnboarding;

namespace FinPilot.IntegrationTests;

public sealed class GetOnboardingByIdTests : IClassFixture<FinPilotWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetOnboardingByIdTests(FinPilotWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_WhenOnboardingExists_ReturnsOnboarding()
    {
        var createRequest = new
        {
            IdentityNumber = "22222222222",
            Email = "get-test@example.com",
            PhoneNumber = "+905551234568"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/onboardings",
            createRequest);

        createResponse.EnsureSuccessStatusCode();

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<StartOnboardingResponse>();

        Assert.NotNull(created);

        var getResponse = await _client.GetAsync(
            $"/api/onboardings/{created.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        var onboarding =
            await getResponse.Content
                .ReadFromJsonAsync<GetOnboardingByIdResponse>();

        Assert.NotNull(onboarding);

        Assert.Equal(created.Id, onboarding.Id);
        Assert.Equal("22222222222", onboarding.IdentityNumber);
        Assert.Equal("get-test@example.com", onboarding.Email);
        Assert.Equal("+905551234568", onboarding.PhoneNumber);
        Assert.Equal("Started", onboarding.Status);
    }

    [Fact]
    public async Task GetById_WhenOnboardingDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync(
            $"/api/onboardings/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
    
    private sealed record StartOnboardingResponse(
    Guid Id,
    string Status);

    private sealed record GetOnboardingByIdResponse(
        Guid Id,
        string IdentityNumber,
        string Email,
        string PhoneNumber,
        string Status,
        DateTime CreatedAtUtc);
}
