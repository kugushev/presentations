using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Dal
{
    public static class ServicesRegistration
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddScoped<IMutantsRepository, MutantsRepository>();
            services.AddScoped<MutantsContext>();
        }
    }
}