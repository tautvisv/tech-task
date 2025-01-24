using Claims.Application.Services;
using Claims.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class PremiumController : ControllerBase
{
    private readonly IPremiumService _service;
    public PremiumController(IPremiumService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet("{coverType}")]
    [SwaggerOperation(Summary = "Compute premium", Description = "Calculates the premium amount based on cover type and date range.")]
    public async Task<ActionResult<decimal>> ComputePremiumAsync([FromRoute] CoverType coverType, DateOnly startDate, DateOnly endDate)
    {
        var premiumValue = await _service.ComputePremiumAsync(startDate, endDate, coverType);
        return Ok(premiumValue);
    }
} 
