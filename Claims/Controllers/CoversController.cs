using AutoMapper;
using Claims.Auditing;
using Claims.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ClaimsContext _claimsContext;
    private readonly Auditer _auditer;
    private readonly IMapper _mapper;

    public CoversController(ClaimsContext claimsContext, AuditContext auditContext, ILogger<CoversController> logger, IMapper mapper)
    {
        _claimsContext = claimsContext;
        _auditer = new Auditer(auditContext);
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoverDto>>> GetAsync()
    {
        var covers = await _claimsContext.Covers.ToListAsync();
        var resilt = _mapper.Map<IEnumerable<Cover>, IEnumerable<CoverDto>>(covers);
        return Ok(resilt);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CoverDto>> GetAsync(string id)
    {
        var covers = await _claimsContext.Covers.ToListAsync();
        var cover = covers.SingleOrDefault(cover => cover.Id == id);
        var result = _mapper.Map<Cover, CoverDto>(cover);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(NewCoverDto request)
    {
        var cover = new Cover();
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        _claimsContext.Covers.Add(cover);
        await _claimsContext.SaveChangesAsync();
        _auditer.AuditCover(cover.Id, "POST");
        var result = _mapper.Map<Cover, CoverDto>(cover);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        _auditer.AuditCover(id, "DELETE");
        var cover = await _claimsContext.Covers.Where(cover => cover.Id == id).SingleOrDefaultAsync();
        if (cover is not null)
        {
            _claimsContext.Covers.Remove(cover);
            await _claimsContext.SaveChangesAsync();
        }
    }

    private decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = (endDate - startDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}
