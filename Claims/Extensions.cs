using Claims.Auditing;
using Claims.Domain.Models;
using Claims.Domain.Repositories;
using Claims.Domain.Services;

namespace Claims
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuditPublisher, AuditPublisher>();

            services.AddScoped<Auditer>();
            
            services.AddScoped<IRepository<Claim>, ClaimsRepository>();
            services.AddScoped<IRepository<Cover>, CoversRepository>();

            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ICoverService, CoverService>();
            services.AddScoped<IPremiumService, CoverPremiumService>();
            return services;
        }
    }
}
