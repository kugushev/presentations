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
using Xunit;

namespace MutantsCatalogue.Tests.Functional
{
    public class MutantsFeature : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory factory;

        public MutantsFeature(TestWebApplicationFactory factory)
        {
            this.factory = factory;
        }


        [Fact]
        public async Task GetMutant_0003_Wolverine()
        {
            // arrange
            var options = new DbContextOptionsBuilder<MutantsContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetMutant_0003_Wolverine))
                .Options;

            using (var context = new MutantsContext(options))
            {
                context.Mutants.Add(new Mutant
                {
                    Name = "Wolverine",
                    RealName = "Logan",
                    Superpower = "Invulnerability, Claws"
                });
                context.SaveChanges();
            }
            
            var currentfactory = factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(
                    services => services.AddSingleton(new MutantsContext(options))));
            
            var client = currentfactory.CreateClient();

            // act
            var response = await client.GetAsync("api/mutants/Wolverine");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<Mutant>();
            Assert.Equal("Wolverine", result.Name);
            Assert.Equal("Logan", result.RealName);
            Assert.Equal("Invulnerability, Claws", result.Superpower);
        }
    }
}
