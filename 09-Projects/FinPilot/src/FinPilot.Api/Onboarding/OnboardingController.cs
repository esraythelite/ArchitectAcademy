using FinPilot.Application.Onboarding.StartOnboarding;
using Microsoft.AspNetCore.Mvc;
using FinPilot.Application.Onboarding.GetOnboardingById;    

namespace FinPilot.Api.Onboarding;

[ApiController]
[Route("api/onboardings")]
public sealed class OnboardingController : ControllerBase
{
    private readonly StartOnboardingHandler _handler;
    private readonly GetOnboardingByIdHandler _getOnboardingByIdHandler;

    public OnboardingController(StartOnboardingHandler handler, GetOnboardingByIdHandler getOnboardingByIdHandler)
    {
        _handler = handler;
        _getOnboardingByIdHandler = getOnboardingByIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartOnboardingRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest("Invalid request");
        }

        var command = new StartOnboardingCommand(
            request.IdentityNumber,
            request.Email,
            request.PhoneNumber);

        var result = await _handler.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getOnboardingByIdHandler.HandleAsync(id, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
