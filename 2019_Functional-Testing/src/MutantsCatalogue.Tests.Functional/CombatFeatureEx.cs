using MutantsCatalogue.Domain.Combat;
using MutantsCatalogue.Tests.Functional.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MutantsCatalogue.Tests.Functional
{
    public class CombatFeatureEx : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory factory;

        public CombatFeatureEx(TestWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Combat_AnyMutant_ReturnsCopiright()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var response = await client.GetAsync("api/combat");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<CombatResult>();
            Assert.Equal("Aleksandr Kugushev 42", result.Copyright);
        }
    }
}
