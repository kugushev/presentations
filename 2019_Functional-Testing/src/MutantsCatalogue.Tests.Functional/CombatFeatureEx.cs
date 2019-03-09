using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Domain.Combat;
using MutantsCatalogue.Tests.Functional.Utils;
using NSubstitute;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MutantsCatalogue.Tests.Functional
{
    public class CombatFeatureEx : IDisposable
    {
        private readonly TestWebApplicationFactory factory;

        public CombatFeatureEx()
        {
            factory = new TestWebApplicationFactory();
        }

        [Fact]
        public async Task Combat_0002_AnyMutant_ReturnsCopiright()
        {
            // arrange
            var client = factory.CreateClient();

            // act
            var response = await client.GetAsync("api/combat?attacker=Magneto&defender=Xavier");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<CombatResult>();
            Assert.Equal("Aleksandr Kugushev 42", result.Copyright);
        }

        [Fact]
        public async Task CombatEpic_0004_AnyMutant_ReturnsVictoryPhrase()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://quotes.rest/qod")
                .Respond("application/json", @"{
  'contents': {
    'quotes': [
      {
        'quote': 'The answer is 42'
      }
    ]
  }
}");
            var httpClientfactory = Substitute.For<IHttpClientFactory>();
            httpClientfactory.CreateClient(Arg.Any<string>()).Returns(mockHttp.ToHttpClient());
            var currentfactory = factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(
                    services => services.AddSingleton(httpClientfactory)));

            var client = currentfactory.CreateClient();

            // act
            var response = await client.GetAsync("api/combat/epic?attacker=Magneto&defender=Xavier");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<CombatResult>();
            Assert.Equal("The answer is 42", result.VictoryPhrase);
        }


        [Fact]
        public async Task CombatEpic_0005_Wolverine_ReturnsVictoryPhraseAboutLive()
        {
            // arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://quotes.rest/qod")
                .WithQueryString("category", "life")
                .Respond("application/json", @"{
  'contents': {
    'quotes': [
      {
        'quote': 'Life is life'
      }
    ]
  }
}");
            var httpClientfactory = Substitute.For<IHttpClientFactory>();
            httpClientfactory.CreateClient(Arg.Any<string>()).Returns(mockHttp.ToHttpClient());
            var currentfactory = factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(
                    services => services.AddSingleton(httpClientfactory)));

            var client = currentfactory.CreateClient();

            // act
            var response = await client.GetAsync("api/combat/epic?attacker=Wolverine&defender=Beast");

            // assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<CombatResult>();
            Assert.Equal("Life is life", result.VictoryPhrase);
        }

        public void Dispose()
        {
            factory.Dispose();
        }
    }
}
