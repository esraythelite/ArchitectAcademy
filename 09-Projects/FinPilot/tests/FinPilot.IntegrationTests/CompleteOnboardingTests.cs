using System.Net;
using System.Net.Http.Json;
using FinPilot.IntegrationTests.Infrastructure;
namespace FinPilot.IntegrationTests;

public sealed class CompleteOnboardingTests : IClassFixture<FinPilotWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CompleteOnboardingTests(FinPilotWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Complete_WhenIdentityVerified_CompletesOnboarding()
    {
        var createRequest = new
        {
            IdentityNumber = "55555555555",
            Email = "complete-integration@example.com",
            PhoneNumber = "+905551234571"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/onboardings",
            createRequest);

        createResponse.EnsureSuccessStatusCode();

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<StartOnboardingResponse>();

        Assert.NotNull(created);

        var verifyResponse = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/verify-identity",
            null);

        Assert.Equal(
            HttpStatusCode.OK,
            verifyResponse.StatusCode);

        var completeResponse = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/complete",
            null);

        Assert.Equal(
            HttpStatusCode.OK,
            completeResponse.StatusCode);

        var completed =
            await completeResponse.Content
                .ReadFromJsonAsync<CompleteOnboardingResponse>();

        Assert.NotNull(completed);
        Assert.Equal(created.Id, completed.Id);
        Assert.Equal("Completed", completed.Status);

        var getResponse = await _client.GetAsync(
            $"/api/onboardings/{created.Id}");

        getResponse.EnsureSuccessStatusCode();

        var onboarding =
            await getResponse.Content
                .ReadFromJsonAsync<GetOnboardingByIdResponse>();

        Assert.NotNull(onboarding);
        Assert.Equal("Completed", onboarding.Status);
    }

    [Fact]
    public async Task Complete_WhenIdentityNotVerified_ReturnsConflict()
    {
        var createRequest = new
        {
            IdentityNumber = "66666666666",
            Email = "complete-conflict@example.com",
            PhoneNumber = "+905551234572"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/onboardings",
            createRequest);

        createResponse.EnsureSuccessStatusCode();

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<StartOnboardingResponse>();

        Assert.NotNull(created);

        var completeResponse = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/complete",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            completeResponse.StatusCode);
    }


    [Fact]
    public async Task Complete_WhenAlreadyCompleted_ReturnsConflict()
    {
        var createRequest = new
        {
            IdentityNumber = "77777777777",
            Email = "complete-conflict@example.com",
            PhoneNumber = "+905551234573"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/onboardings",
            createRequest);

        createResponse.EnsureSuccessStatusCode();

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<StartOnboardingResponse>();

        Assert.NotNull(created);

        var verifyResponse = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/verify-identity",
            null);

        Assert.Equal(
            HttpStatusCode.OK,
            verifyResponse.StatusCode);

        var completeResponse = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/complete",
            null);

         Assert.Equal(
            HttpStatusCode.OK,
            completeResponse.StatusCode);

        var completeResponseSecond = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/complete",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            completeResponseSecond.StatusCode);
    }
    private sealed record CompleteOnboardingResponse(
    Guid Id,
    string Status);

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

