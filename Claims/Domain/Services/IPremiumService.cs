using Claims.Domain.Models;

namespace Claims.Domain.Services
{
    public interface IPremiumService
    {
        Task<decimal> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType);
    }
}
