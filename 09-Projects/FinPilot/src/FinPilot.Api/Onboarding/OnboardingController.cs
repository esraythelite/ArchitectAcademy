using FinPilot.Application.Onboarding.StartOnboarding;
using Microsoft.AspNetCore.Mvc;

namespace FinPilot.Api.Onboarding;

[ApiController]
[Route("api/onboardings")]
public sealed class OnboardingController : ControllerBase
{
    private readonly StartOnboardingHandler _handler;

    public OnboardingController(StartOnboardingHandler handler)
    {
        _handler = new StartOnboardingHandler();
    }

    [HttpPost]
    public IActionResult Start([FromBody] StartOnboardingRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request");
        }

        var command = new StartOnboardingCommand(
            request.IdentityNumber,
            request.Email,
            request.PhoneNumber);

        var result = _handler.Handle(command);

        if (result == null)
        {
            return StatusCode(500, "An error occurred while processing your request");
        }

        return CreatedAtAction(nameof(Start), result);
    }
}
