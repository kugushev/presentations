using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Domain.Combat;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Domain
{
    public static class ServicesRegistration
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ICombatService, CombatService>();
            services.AddScoped<IMutantsService, MutantsService>();
        }
        
    }
}