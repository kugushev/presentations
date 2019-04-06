using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MutantsCatalogue.Application;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using MutantsCatalogue.Dal;

namespace MutantsCatalogue.Tests.Functional.Utils
{
    public class TestWebApplicationFactory : WebApplicationFactory<TestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<TestStartup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot("../../../../MutantsCatalogue.Application");
            var config = new Dictionary<string, string>
            {
                ["Domain:CopyrightYear"] = "42"
            };
            builder.ConfigureAppConfiguration(b => b.AddInMemoryCollection(config));

            base.ConfigureWebHost(builder);
        }        
    }
}
