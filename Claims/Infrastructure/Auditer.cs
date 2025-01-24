using Claims.Infrastructure.Persistance;
using Claims.Infrastructure.Persistance.Models;
using Claims.Utils;

namespace Claims.Infrastructure
{
    public class Auditer
    {
        private readonly AuditContext _auditContext;
        private readonly IDateTimeService _dateTimeService;

        public Auditer(AuditContext auditContext, IDateTimeService dateTimeService)
        {
            _auditContext = auditContext ?? throw new ArgumentNullException(nameof(auditContext));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        }

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = _dateTimeService.GetCurrentTime(),
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditContext.Add(claimAudit);
            _auditContext.SaveChanges();
        }

        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = _dateTimeService.GetCurrentTime(),
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditContext.Add(coverAudit);
            _auditContext.SaveChanges();
        }
    }
}
