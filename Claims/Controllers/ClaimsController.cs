using Claims.Auditing;
using Claims.Controllers.Models;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly ClaimsContext _claimsContext;
        private readonly Auditer _auditer;

        public ClaimsController(ILogger<ClaimsController> logger, ClaimsContext claimsContext, AuditContext auditContext)
        {
            _logger = logger;
            _claimsContext = claimsContext;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> GetAsync()
        {
            IEnumerable<Claim> claims = await _claimsContext.GetClaimsAsync();
            return Ok(claims);
        }

        [HttpPost]
        public async Task<ActionResult<ClaimDto>> CreateAsync(NewClaimDto request)
        {
            var claim = new Claim
            {
                Name = request.Name,
            };
            claim.Id = Guid.NewGuid().ToString();
            await _claimsContext.AddItemAsync(claim);
            _auditer.AuditClaim(claim.Id, "POST");
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            _auditer.AuditClaim(id, "DELETE");
            await _claimsContext.DeleteItemAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimDto>> GetAsync(string id)
        {
            var claim = await _claimsContext.GetClaimAsync(id);
            return Ok(claim);
        }
    }
}
