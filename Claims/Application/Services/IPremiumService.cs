using Claims.Domain.Models;

namespace Claims.Application.Services
{
    public interface IPremiumService
    {
        Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}
