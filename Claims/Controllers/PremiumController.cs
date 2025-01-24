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
    [ProducesResponseType(typeof(decimal), 200)]
    public async Task<IActionResult> ComputePremiumAsync([FromRoute] CoverType coverType, DateTime startDate, DateTime endDate)
    {
        var premiumValue = await _service.ComputePremiumAsync(startDate, endDate, coverType);
        return Ok(premiumValue);
    }
} 
