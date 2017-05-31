using System;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;

namespace VotingApp.Domain
{
    public class VotingCommandsService
    {
        private readonly IRepository _repo;

        public VotingCommandsService(IRepository repo) => _repo = repo;

        public async Task<VotingProjection> StartVoting(string[] topics) =>
            await ExecuteCommand(voting => voting.Start(topics));

        public async Task<VotingProjection> Vote(Guid votingId, string topic) =>
            await ExecuteCommand(voting => voting.Vote(topic), votingId);

         public async Task<VotingProjection> FinishVoting(Guid votingId) =>
            await ExecuteCommand(voting => voting.Finish(), votingId);

        private async Task<VotingProjection> ExecuteCommand(Action<VotingAggregate> command, Nullable<Guid> id = null)
        {   
            var voting = (id == null) 
                ? new VotingAggregate() : 
                await _repo.GetById<VotingAggregate>(id.Value);

            command(voting);
            await _repo.Save(voting);
            return voting.State;
        }
    }
}