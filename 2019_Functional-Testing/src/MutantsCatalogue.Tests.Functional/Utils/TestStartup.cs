using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MutantsCatalogue.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutantsCatalogue.Tests.Functional.Utils
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration) { }

        protected override void ConfigureUtilityMiddlewares(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        protected override void ConfigureUtilityServices(IServiceCollection services)
        {
            
        }
    }
}
