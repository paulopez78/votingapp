using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EasyEventSourcing;
using EasyWebSockets;
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
        private readonly VotingQueriesService _queries;
        private ConcurrentDictionary<Guid, VotingStatsProjection> _votingStats;
        private readonly IWebSocketPublisher _wsPublisher;

        public VotingResultsService(IEventStoreBus eventStoreBus,
            IWebSocketPublisher wsPublisher,
            VotingQueriesService queries,
            ILogger<VotingResultsService> logger)
        {
            _eventStoreBus = eventStoreBus;
            _wsPublisher = wsPublisher;
            _queries = queries;
            _logger = logger;
            _votingStats = new ConcurrentDictionary<Guid, VotingStatsProjection>();
        }

        public void Start()
        {
            _eventStoreBus.Subscribe<VotingAggregate>(async (aggregateId, @event) =>
            {
                _logger.LogInformation(@event.ToString());
                await AddOrUpdateVotingStats(aggregateId, @event);
                await _wsPublisher.SendMessageToAllAsync(_votingStats[aggregateId]);
            })
            .Wait();
        }

        private async Task AddOrUpdateVotingStats(Guid aggregateId, object @event)
        {
            if (_votingStats.TryGetValue(aggregateId, out VotingStatsProjection stats))
                _votingStats[aggregateId] = stats.Reduce(@event);
            else
                _votingStats[aggregateId] = await _queries.GetVotingStats(aggregateId);
        }
    }
}