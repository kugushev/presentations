using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MutantsCatalogue.Domain.Combat;

namespace MutantsCatalogue.Infrastructure
{
    internal class QuotesProxy : IQuotesProxy
    {
        private readonly IHttpClientFactory clientFactory;

        public QuotesProxy(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        
        public async Task<string> GetQuoteAsync(string category)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"https://quotes.rest/qod");
            request.Headers.Add("Accept", "application/json");

            if (category != null)
                request.Properties.Add("category", category);
                
            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Result>();
                return result?.Contents?.Quotes?.FirstOrDefault()?.Quote;
            }

            return null;
        }
    }
}
