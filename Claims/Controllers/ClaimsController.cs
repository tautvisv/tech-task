using AutoMapper;
using Claims.Controllers.Models;
using Claims.Domain.Models;
using Claims.Domain.Services;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _service;
        private readonly IMapper _mapper;

        public ClaimsController(IClaimService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetAsync()
        {
            var claims = await _service.GetClaimsAsync();
            var result = _mapper.Map<IEnumerable<Claim>, IEnumerable<ClaimDto>>(claims);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ClaimDto>> CreateAsync(NewClaimDto request)
        {
            var newClaim = _mapper.Map<NewClaimDto, NewClaim>(request);
            var claim = await _service.CreateClaimAsync(newClaim);
            var result = _mapper.Map<Claim, ClaimDto>(claim);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _service.DeleteClaimAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimDto>> GetAsync(string id)
        {
            var claim = await _service.GetClaimAsync(id);
            var result = _mapper.Map<Claim, ClaimDto>(claim);
            return Ok(result);
        }
    }
}
