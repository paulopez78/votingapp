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

        [HttpGet]
        public async Task<VotingProjection> Get() => 
            await _queries.GetVoting();

        [HttpGet("{votingId}")]
        public async Task<VotingProjection> Get(Guid votingId) => 
            await _queries.GetVoting(votingId);

        [HttpPost]
        public async Task<VotingProjection> Post([FromBody]string[] topics) =>
            await _commands.StartVoting(topics.Select(x => x.ToLower()).ToArray());

        [HttpPut("{votingId}")]
        public async Task<VotingProjection> Put(Guid votingId, [FromBody]string topic) =>
            await _commands.Vote(votingId, topic.ToLower());

        [HttpDelete("{votingId}")]
        public async Task<VotingProjection> Delete(Guid votingId) =>
            await _commands.FinishVoting(votingId);
    }
}
