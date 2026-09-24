using FinPilot.Application.Onboarding.StartOnboarding;
using Microsoft.AspNetCore.Mvc;
using FinPilot.Application.Onboarding.GetOnboardingById;
using FinPilot.Application.Onboarding.VerifyIdentity;
using FinPilot.Application.Onboarding.CompleteOnboarding;

namespace FinPilot.Api.Onboarding;

[ApiController]
[Route("api/onboardings")]
public sealed class OnboardingController : ControllerBase
{
    private readonly StartOnboardingHandler _handler;
    private readonly GetOnboardingByIdHandler _getOnboardingByIdHandler;
    private readonly VerifyIdentityHandler _verifyIdentityHandler;
    private readonly CompleteOnboardingHandler _completeOnboardingHandler;

    public OnboardingController(StartOnboardingHandler handler, GetOnboardingByIdHandler getOnboardingByIdHandler, VerifyIdentityHandler verifyIdentityHandler, CompleteOnboardingHandler completeOnboardingHandler)
    {
        _handler = handler;
        _getOnboardingByIdHandler = getOnboardingByIdHandler;
        _verifyIdentityHandler = verifyIdentityHandler;
        _completeOnboardingHandler = completeOnboardingHandler;
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

    [HttpPost("{id:guid}/verify-identity")]
    public async Task<IActionResult> VerifyIdentity(Guid id, CancellationToken cancellationToken)
    {
        var command = new VerifyIdentityCommand(id);

        var result = await _verifyIdentityHandler.HandleAsync(command, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var command = new CompleteOnboardingCommand(id);

        var result = await _completeOnboardingHandler.HandleAsync(command, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

}
