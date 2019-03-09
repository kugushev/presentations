using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MutantsCatalogue.Application;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MutantsCatalogue.Tests.Functional.Utils
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var config = new Dictionary<string, string>
            {
                ["Domain:CopyrightYear"] = "42"
            };
            builder.ConfigureAppConfiguration(b => b.AddInMemoryCollection(config));
            base.ConfigureWebHost(builder);
        }
    }
}
