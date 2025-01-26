using Claims.Domain.Models;

namespace Claims.Application.Services
{
    /// <summary>
    /// IPremiumService service computes premium prices
    /// </summary>
    public interface IPremiumService
    {
        /// <summary>
        /// ComputePremiumAsync by perion duration and cover type
        /// </summary>
        /// <param name="startDate">Period start</param>
        /// <param name="endDate">Period end</param>
        /// <param name="coverType">Cover type which influence base price and discounts</param>
        /// <returns></returns>
        Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}
