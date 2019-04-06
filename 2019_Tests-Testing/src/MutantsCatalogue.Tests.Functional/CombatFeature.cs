using Microsoft.AspNetCore.Mvc.Testing;
using MutantsCatalogue.Application;
using MutantsCatalogue.Domain.Combat;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MutantsCatalogue.Tests.Functional
{
    public class CombatFeature : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public CombatFeature(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Combat_0001_MagnetoVsWolverine_MagnetoWins()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var response = await client.GetAsync("api/combat?attacker=Magneto&defender=Wolverine");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<CombatResult>();
            Assert.Equal("Magneto", result.Winner);
        }
    }
}
