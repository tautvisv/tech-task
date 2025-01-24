using Claims.Auditing;
using Claims.Domain.Models;
using Claims.Domain.Repositories;
using Claims.Domain.Services;
using Claims.Infrastructure.Messaging;
using Claims.Utils;

namespace Claims
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, UtcDateTimeService>();
            services.AddBackgroundServices();
            services.AddScoped<IAuditPublisher, AuditPublisher>();

            services.AddScoped<Auditer>();
            
            services.AddScoped<IRepository<Claim>, ClaimsRepository>();
            services.AddScoped<IRepository<Cover>, CoversRepository>();

            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ICoverService, CoverService>();
            services.AddScoped<IPremiumService, CoverPremiumService>();
            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(Program).Assembly));
            return services;
        }
    }
}
