using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Domain;

namespace VotingApp.Api
{
    [Route("api/[controller]")]
    public class VotingStatsController : Controller
    {
        private readonly VotingQueriesService _queries;

        public VotingStatsController(VotingQueriesService queries) =>
            _queries = queries;

        [HttpGet]
        public async Task<VotingStatsProjection> Get() => 
            await _queries.GetVotingStats();

        [HttpGet("{votingId}")]
        public async Task<VotingStatsProjection> Get(Guid votingId) => 
            await _queries.GetVotingStats(votingId);
    }
}
