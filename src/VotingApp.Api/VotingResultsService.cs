using System;
using System.Collections.Concurrent;
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
        private readonly VotingQueriesService _queries;
        private ConcurrentDictionary<Guid, VotingStatsProjection> _votingStats;

        public VotingResultsService(IEventStoreBus eventStoreBus,
            VotingQueriesService queries,
            ILogger<VotingResultsService> logger)
        {
            _eventStoreBus = eventStoreBus;
            _queries = queries;
            _logger = logger;
            _votingStats = new ConcurrentDictionary<Guid, VotingStatsProjection>();
        }

        public void Start()
        {
            _eventStoreBus.Subscribe<VotingAggregate>((Func<Guid, object, Task>)(async (aggregateId, @event) =>
            {
                switch (@event)
                {
                    case VotingStartedEvent startedEvent:
                        _votingStats[aggregateId] = await _queries.GetVotingStats(aggregateId);
                        break;

                    case TopicVotedEvent votedEvent:
                    case VotingFinishedEvent finishedEvent:
                        await AddOrUpdate(aggregateId, @event);
                        break;

                    default:
                        break;
                }
                _logger.LogInformation(@event.ToString());

                // TODO: Broadcast stats using websockets to clients
            }))
            .Wait();

            async Task AddOrUpdate(Guid aggregateId, object @event)
            {
                if (_votingStats.TryGetValue(aggregateId, out VotingStatsProjection stats))
                    _votingStats[aggregateId] = stats.Reduce(@event);
                else
                    _votingStats[aggregateId] = await _queries.GetVotingStats(aggregateId);
            }
        }
    }
}