using Claims.Application.Models;
using Claims.Domain.Models;

namespace Claims.Application.Services
{
    // It is possible to have a generic interface and use it for covers and claims but code is a bit simpler with interfaces which are duplicated.
    // Non generic interface gives a bit better code readability and simplicity.
    public interface ICoverService
    {
        public Task<Cover> GetCoverAsync(string id);
        public Task<IEnumerable<Cover>> GetCoversAsync();
        public Task<Cover> CreateCoverAsync(NewCover newClaim);
        public Task DeleteCoverAsync(string id);
    }
}
