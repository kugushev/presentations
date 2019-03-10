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
        public void Combat_0001_MagnetoVsWolverine_MagnetoWins(HttpResponseMessage response)
        {
            const string url = "api/combat?attacker=Magneto&defender=Wolverine";
            $"When GET {url}".x(async () => response = await factory.CreateClient().GetAsync(url));
            
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
