using System.Threading.Tasks;
using EasyEventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using VotingApp.Domain;

namespace VotingApp.Api
{
    public static class VotingResultsServiceExtensions
    {
        public static IApplicationBuilder UseVotingResultsService(this IApplicationBuilder app)
        {
            var votingResult = (VotingResultsService)app.ApplicationServices.GetService(typeof(VotingResultsService));
            votingResult.Start();
            return app;
        }
    }
    public class VotingResultsService
    {
        private readonly IEventStoreBus _eventStoreBus;
        private readonly ILogger<VotingResultsService> _logger;

        public VotingResultsService(IEventStoreBus eventStoreBus, ILogger<VotingResultsService> logger)
        {
            _eventStoreBus = eventStoreBus;
            _logger = logger;
        }

        public void Start() =>
            _eventStoreBus.Subscribe<VotingAggregate>(
                async (@event) =>
                {
                    _logger.LogInformation(@event.ToString());
                    await Task.FromResult(true);
                })
                .Wait();
    }
}