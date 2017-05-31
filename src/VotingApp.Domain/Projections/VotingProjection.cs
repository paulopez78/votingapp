using System;
using System.Collections.Generic;
using System.Linq;

namespace VotingApp.Domain
{
    public class VotingProjection
    {
        public IDictionary<string, int> Topics { get; private set; }

        public string Winner { get; private set; }

        public VotingProjection(IDictionary<string, int> topics, string winner = "")
        {
            Topics = topics;
            Winner = winner;
        }

        public static VotingProjection Empty() => 
            new VotingProjection(new Dictionary<string,int>());

        public VotingProjection With(string winner) =>
            new VotingProjection(this.Topics, winner);

        public VotingProjection With(IDictionary<string, int> topics) =>
            new VotingProjection(topics, this.Winner);
    }

    public static class VotingProjectionExtensions
    {
        public static VotingProjection Reduce(this VotingProjection state, object @event)
        {
            state = state ?? VotingProjection.Empty();
            switch (@event)
            {
                case VotingStartedEvent votingStarted:
                    return state.With(votingStarted.Topics.ToDictionary(topic => topic, _ => 0));
                    
                case TopicVotedEvent voted:
                    return state.With(state.Topics.ToDictionary(
                        t => t.Key,
                        t => t.Key == voted.Topic
                                ? t.Value + 1
                                : t.Value));
                                
                case VotingFinishedEvent votingFinished:
                    return state.With(votingFinished.Winner);
                    
                default:
                    return state;
            }
        }
    }
}
