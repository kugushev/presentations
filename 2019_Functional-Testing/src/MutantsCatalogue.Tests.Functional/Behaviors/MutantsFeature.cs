using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Dal;
using MutantsCatalogue.Domain.Mutants;
using MutantsCatalogue.Tests.Functional.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace MutantsCatalogue.Tests.Functional.Behaviors
{
    public class MutantsFeature
    {
        private readonly TestWebApplicationFactory factory;

        public MutantsFeature()
        {
            factory = new TestWebApplicationFactory();
        }

        [Scenario]
        public void Scenario_GetMutant_0003_Wolverine(DbContextOptions<MutantsContext> options, WebApplicationFactory<TestStartup> currentfactory, 
            HttpResponseMessage response)
        {
            $"Given database".x(() =>
            {
                options = new DbContextOptionsBuilder<MutantsContext>()
                .UseInMemoryDatabase(databaseName: nameof(Scenario_GetMutant_0003_Wolverine))
                .Options;
            });

            var mutant = new Mutant
            {
                Name = "Wolverine",
                RealName = "Logan",
                Superpower = "Invulnerability, Claws"
            };
            $"And the mutant {mutant}".x(() =>
            {
                using (var context = new MutantsContext(options))
                {
                    context.Mutants.Add(mutant);
                    context.SaveChanges();
                }
                currentfactory = factory
                    .WithWebHostBuilder(builder => builder.ConfigureTestServices(
                        services => services.AddSingleton(new MutantsContext(options))));
            });
                        
            $"When get mutant by name Wolverine"
                .x(async () => response = await currentfactory.CreateClient().GetAsync("api/mutants/Wolverine"));

            "Then returns the mutant Wolverine (Logan): Invulnerability, Claws"
                .x(async () =>
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsAsync<Mutant>();
                    Assert.Equal("Wolverine", result.Name);
                    Assert.Equal("Logan", result.RealName);
                    Assert.Equal("Invulnerability, Claws", result.Superpower);
                });
        }
    }
}
