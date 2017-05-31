using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Domain;

namespace VotingApp.Api
{
    [Route("api/[controller]")]
    public class VotingController : Controller
    {
        private readonly VotingCommandsService _commands;
        private readonly VotingQueriesService _queries;

        public VotingController(VotingCommandsService commands, VotingQueriesService queries) 
        {
            _commands = commands;
            _queries = queries;
        }

        [HttpGet("{votingId}")]
        public async Task<VotingProjection> Get(Guid votingId) => 
            await _queries.GetVoting(votingId);

        [HttpGet("{votingId}/stats")]
        public async Task<VotingStatsProjection> GetStats(Guid votingId) => 
            await _queries.GetVotingStats(votingId);

        [HttpPost]
        public async Task<VotingProjection> Post([FromBody]string[] topics) =>
            await _commands.StartVoting(topics);

        [HttpPut("{votingId}")]
        public async Task<VotingProjection> Put(Guid votingId, [FromBody]string topic) =>
            await _commands.Vote(votingId, topic);

        [HttpDelete("{votingId}")]
        public async Task<VotingProjection> Delete(Guid votingId) =>
            await _commands.FinishVoting(votingId);
    }
}
