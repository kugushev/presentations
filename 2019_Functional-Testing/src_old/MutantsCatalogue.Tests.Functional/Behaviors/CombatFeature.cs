using MutantsCatalogue.Domain.Combat;
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
    public class CombatFeature : IDisposable
    {
        private readonly TestWebApplicationFactory factory;

        public CombatFeature()
        {
            factory = new TestWebApplicationFactory();
        }

        [Scenario]
        public void Scenario_Combat_0001_MagnetoVsWolverine_MagnetoWins(HttpResponseMessage response)
        {            
            $"When combat between Magneto and Wolverine"
                .x(async () => response = await factory.CreateClient().GetAsync("api/combat?attacker=Magneto&defender=Wolverine"));
            
            "Then winner is Magneto".x(async () =>
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<CombatResult>();
                Assert.Equal("Magneto", result.Winner);
            });            
        }

        public void Dispose()
        {
            factory.Dispose();
        }
    }
}
