using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MutantsCatalogue.Dal;
using MutantsCatalogue.Domain;
using MutantsCatalogue.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace MutantsCatalogue.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpClient();
            ConfigureUtilityServices(services);

            ConfigureSettings(services);
            
            services.AddDomain();
            services.AddDal();
            services.AddInfrastructure();
        }

        protected virtual void ConfigureUtilityServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = Configuration.GetValue<string>("Api:Name"),
                    Version = Configuration.GetValue<string>("Api:Version"),
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureUtilityMiddlewares(app, env);
            app.UseMvc();
        }

        protected virtual void ConfigureUtilityMiddlewares(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration.GetValue<string>("Api:Name"));
            });

            app.UseHttpsRedirection();
        }

        private void ConfigureSettings(IServiceCollection services)
        {
            var settings = new DomainSettings(
                Configuration.GetValue<string>("Domain:Copyright"),
                Configuration.GetValue<string>("Domain:CopyrightYear"));
            services.AddSingleton(settings);
        }
    }
}
