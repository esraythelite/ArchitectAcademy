
using System.Net;
using System.Net.Http.Json;
using FinPilot.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Identity;
using FinPilot.Application.Onboarding.StartOnboarding;
namespace FinPilot.IntegrationTests;

public sealed class VerifyIdentityTests : IClassFixture<FinPilotWebApplicationFactory>
{
    private readonly HttpClient _client;

    public VerifyIdentityTests(FinPilotWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task VerifyIdentity_WhenOnboardingExists_UpdatesStatus()
    {
        var createRequest = new
        {
            IdentityNumber = "33333333333",
            Email = "verify-integration@example.com",
            PhoneNumber = "+905551234569"
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

        var verified =
            await verifyResponse.Content
                .ReadFromJsonAsync<VerifyIdentityResponse>();

        Assert.NotNull(verified);
        Assert.Equal(created.Id, verified.Id);
        Assert.Equal("IdentityVerified", verified.Status);

        var getResponse = await _client.GetAsync(
            $"/api/onboardings/{created.Id}");

        getResponse.EnsureSuccessStatusCode();

        var onboarding =
            await getResponse.Content
                .ReadFromJsonAsync<GetOnboardingByIdResponse>();

        Assert.NotNull(onboarding);
        Assert.Equal(
            "IdentityVerified",
            onboarding.Status);
    }

    [Fact]
    public async Task VerifyIdentity_WhenOnboardingDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.PostAsync(
            $"/api/onboardings/{Guid.NewGuid()}/verify-identity",
            null);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task VerifyIdentity_WhenAlreadyVerified_ReturnsConflict()
    {
        var createRequest = new
        {
            IdentityNumber = "44444444444",
            Email = "verify-conflict@example.com",
            PhoneNumber = "+905551234570"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/onboardings",
            createRequest);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<StartOnboardingResponse>();

        Assert.NotNull(created);

        var firstVerify = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/verify-identity",
            null);

        Assert.Equal(
            HttpStatusCode.OK,
            firstVerify.StatusCode);

        var secondVerify = await _client.PostAsync(
            $"/api/onboardings/{created.Id}/verify-identity",
            null);

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondVerify.StatusCode);
    }

    private sealed record VerifyIdentityResponse(
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

