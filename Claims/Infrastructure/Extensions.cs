using Claims.Application.Repositories;
using Claims.Application.Services;
using Claims.Domain.Models;
using Claims.Infrastructure.Messaging;
using Claims.Infrastructure.Persistance;
using Claims.Utils;

namespace Claims.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, UtcDateTimeService>();
            services.AddBackgroundServices();
            services.AddScoped<AuditPublisher>();
            services.AddScoped<IAuditClaimPublisher>(x => x.GetService<AuditPublisher>()!);
            services.AddScoped<IAuditCoverPublisher>(x => x.GetService<AuditPublisher>()!);

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
