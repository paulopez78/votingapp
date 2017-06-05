using System;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing;

namespace VotingApp.Domain
{
    public class VotingQueriesService
    {
        private readonly IEventStore _eventStore;
        private readonly IEventStoreProjections _projections;

        public VotingQueriesService(IEventStore eventStore, IEventStoreProjections projections)
        {
            _eventStore = eventStore;
            _projections = projections;
        }

        public async Task<VotingStatsProjection> GetVotingStats(Guid? votingId = null) =>
            await Get<VotingStatsProjection>(votingId, VotingStatsProjectionExtensions.Reduce);

        public async Task<VotingProjection> GetVoting(Guid? votingId = null) =>
            await Get<VotingProjection>(votingId, VotingProjectionExtensions.Reduce);

        private async Task<T> Get<T>(Guid? id, Func<T, object, T> reducer) where T : class
        {
            id = id ?? await _projections.GetCurrentId<VotingAggregate>();

            var events = await _eventStore.GetEventStream<VotingAggregate>(id.Value);
            T state = null;
            events.ToList().ForEach(@event => state = reducer(state, @event));
            return state;
        }
    }
}