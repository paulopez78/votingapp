using System;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing;

namespace VotingApp.Domain
{
    public class VotingQueriesService
    {
        private readonly IEventStore _eventStore;

        public VotingQueriesService(IEventStore eventStore) => _eventStore = eventStore;

        public async Task<VotingStatsProjection> GetVotingStats(Guid votingId) =>
            await Get<VotingStatsProjection>(votingId, VotingStatsProjectionExtensions.Reduce);

        public async Task<VotingProjection> GetVoting(Guid votingId) => 
            await Get<VotingProjection>(votingId, VotingProjectionExtensions.Reduce);

        private async Task<T> Get<T> (Guid id, Func<T,object,T> reducer) where T : class
        {
            var events = await _eventStore.GetEventStream<VotingAggregate>(id);
            T state = null;
            events.ToList().ForEach(@event => state = reducer(state, @event));
            return state;    
        }
    }
}