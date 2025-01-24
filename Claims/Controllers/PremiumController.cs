using Claims.Domain.Models;
using Claims.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<decimal>> ComputePremiumAsync([FromRoute] CoverType coverType, DateTime startDate, DateTime endDate)
    {
        var premiumValue = await _service.ComputePremiumAsync(startDate, endDate, coverType);
        return Ok(premiumValue);
    }
} 
