using Core.Data;
using Core.Framework.Attributes;
using Core.Framework.Enums;
using Core.Plugins.Extensions;
using Core.Plugins.Microsoft.Azure.Wrappers;
using Core.Plugins.SQLServer.Wrappers;
using Core.Providers;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

namespace Core.Plugins.Microservices.Application.Bootstrap
{
    public class MicroserviceBootstrapper
    {
        private readonly MicroserviceConfiguration _microserviceConfiguration;

        public MicroserviceBootstrapper(MicroserviceConfiguration microserviceConfiguration)
        {
            _microserviceConfiguration = microserviceConfiguration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc(_microserviceConfiguration.SwaggerConfiguration.Version, new Info
                {
                    Title = _microserviceConfiguration.SwaggerConfiguration.Title,
                    Version = _microserviceConfiguration.SwaggerConfiguration.Version,
                    Description = _microserviceConfiguration.SwaggerConfiguration.Description,
                    TermsOfService = "Not for use outside of Great American Power"
                });
            });

            services.AddMediatR(_microserviceConfiguration.AssembliesToScan);

            services.RegisterAssemblyPublicNonGenericClasses(_microserviceConfiguration.AssembliesToScan)
                .Where(type => !type.GetCustomAttributes(typeof(InjectableAttribute), true).Any() ||
                                type.GetCustomAttributes(typeof(InjectableAttribute), true).Select(ia => ((InjectableAttribute)ia).AutoWiring != Opt.Out).FirstOrDefault())
                .AsPublicImplementedInterfaces();

            services.AddTransient<IDatabaseFactory, SQLServerDatabaseFactory>();
            services.AddTransient<IConnectionStringProvider, AzureConnectionStringByConfigurationProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseHttpsRedirection();
            app.UseMvc();

            loggerFactory.AddAzureWebAppDiagnostics();
            loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pathBase = _microserviceConfiguration.AppSettings["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/{_microserviceConfiguration.SwaggerConfiguration.Version}/swagger.json", $"{_microserviceConfiguration.ServiceName} {_microserviceConfiguration.SwaggerConfiguration.Version.ToUpper()}");
                   c.OAuthClientId($"{_microserviceConfiguration.ServiceName.ToLower().Remove(" ")}swaggerui");
                   c.OAuthAppName($"{_microserviceConfiguration.ServiceName} Swagger UI");
               });
        }
    }
}
