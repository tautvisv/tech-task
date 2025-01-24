using AutoMapper;
using Claims.Controllers.Models;
using Claims.Domain.Models;
using Claims.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICoverService _service;

    public CoversController(ILogger<CoversController> logger, IMapper mapper, ICoverService service)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all covers", Description = "Retrieves a list of all available covers.")]
    public async Task<ActionResult<IEnumerable<CoverDto>>> GetAsync()
    {
        var covers = await _service.GetCoversAsync();
        var resilt = _mapper.Map<IEnumerable<Cover>, IEnumerable<CoverDto>>(covers);
        return Ok(resilt);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a cover by ID", Description = "Retrieves details of a specific cover by its ID.")]
    public async Task<ActionResult<CoverDto>> GetAsync(string id)
    {
        var cover = await _service.GetCoverAsync(id);
        var result = _mapper.Map<Cover, CoverDto>(cover);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new cover", Description = "Creates a new cover and returns the created cover details.")]
    public async Task<ActionResult<CoverDto>> CreateAsync(NewCoverDto request)
    {
        var newCover = _mapper.Map<NewCoverDto, NewCover>(request);
        var cover = await _service.CreateCoverAsync(newCover);
        var result = _mapper.Map<Cover, CoverDto>(cover);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a cover", Description = "Deletes an existing cover by its ID.")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
       await _service.DeleteCoverAsync(id);
       return NoContent();
    }
}
