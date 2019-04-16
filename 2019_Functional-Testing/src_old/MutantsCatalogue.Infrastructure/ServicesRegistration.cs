using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Domain.Combat;

namespace MutantsCatalogue.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IQuotesProxy, QuotesProxy>();
        }
    }
}