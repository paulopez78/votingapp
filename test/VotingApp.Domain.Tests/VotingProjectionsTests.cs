using System;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Domain;
using Xunit;

namespace VotingApp.Tests
{
    public class VotingProjectionsTests
    {
        [Fact]
        public void Given_StreamOfEvents_When_VotingProjection_Reduce_Then_Votes_And_Winner()
        {
            VotingProjection state = null;
            GetEvents().ForEach(@event => state = state.Reduce(@event));

            Assert.Equal(state.Topics, new Dictionary<string, int> { 
                { "C#", 1 }, 
                { "F#", 2 } 
            });
            Assert.Equal(state.Winner, "F#");
        }

        [Fact]
        public void Given_StreamOfEvents_When_VotingStatsProjection_Reduce_Then_VotesPercent_And_Winner()
        {
            VotingStatsProjection state = null;
            GetEvents().ForEach(@event => state = state.Reduce(@event));

            Assert.Equal(state.Votes, new Dictionary<string, (int,int)> { 
                { "C#", (33, 1) }, 
                { "F#", (67, 2) } 
            });
            Assert.Equal(state.Votes.Sum(x => x.Value.percent), 100);
            Assert.Equal(state.Winner, "F#");
        }

        private List<object> GetEvents() => new object[] {
                new VotingStartedEvent("C#", "F#"),
                new TopicVotedEvent("C#"),
                new TopicVotedEvent("F#"),
                new TopicVotedEvent("F#"),
                // check if there is a null
                null, 
                // check if type does not match
                new Dictionary<string,string>() { {"bar", "foo"}}, 
                new VotingFinishedEvent("F#")
            }.ToList();
    }
}
