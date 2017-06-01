using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using VotingApp.Domain;

namespace VotingApp.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Voting API", Version = "v1" })
            );
            services.AddEasyEventSourcing<VotingAggregate>(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"],
                    Configuration["EVENT_STORE_MANAGER_HOST"]));
            services.AddScoped<VotingCommandsService>();
            services.AddScoped<VotingQueriesService>();
        }
        public void Configure(IApplicationBuilder app, IEventStoreBus eventStoreBus, ILogger<Startup> logger)
        {
            app.UseExceptionHandler();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingApp API"));

            eventStoreBus.Subscribe<VotingAggregate>(
                async (@event) =>
                {
                    logger.LogInformation(@event.ToString());
                    await Task.FromResult(true);
                })
                .DefaultRetryAsync()
                .Wait();
        }
    }
}
