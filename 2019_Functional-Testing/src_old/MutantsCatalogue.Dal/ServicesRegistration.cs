using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MutantsCatalogue.Domain.Mutants;
using System;

namespace MutantsCatalogue.Dal
{
    public static class ServicesRegistration
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddScoped<IMutantsRepository, MutantsRepository>();
            services.AddScoped(DbContextFactory);
        }

        private static MutantsContext DbContextFactory(IServiceProvider serviceProvider)
        {
            var options = new DbContextOptionsBuilder<MutantsContext>()
                .UseSqlite("Data Source=mutants.db")
                .Options;
            return new MutantsContext(options);
        }
    }
}