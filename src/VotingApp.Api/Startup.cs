using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VotingApp.Domain;

namespace VotingApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "Voting API", Version = "v1" }))
                    .AddEasyEventSourcing<VotingAggregate>(Configuration)
                    .AddScoped<VotingCommandsService>()
                    .AddScoped<VotingQueriesService>()
                    .AddSingleton<VotingResultsService>()
                    .AddMvc();

        public void Configure(IApplicationBuilder app) =>
            app.UseExceptionHandler()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingApp API"))
                .UseVotingResultsService();
    }
}
