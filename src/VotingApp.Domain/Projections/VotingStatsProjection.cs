using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Domain;

namespace VotingApp.Domain
{
    public class VotingStatsProjection
    {
        public IDictionary<string, (int percent, int votes)> Votes { get; }
        public string Winner { get; }

        public VotingStatsProjection(IDictionary<string, (int, int)> votes, string winner = "")
        {
            Votes = votes;
            Winner = winner;
        }

        public VotingStatsProjection With(string winner) =>
            new VotingStatsProjection(this.Votes, winner);

        public VotingStatsProjection With(IDictionary<string, (int, int)> votes) =>
            new VotingStatsProjection(votes, this.Winner);

        public static VotingStatsProjection Empty() =>
            new VotingStatsProjection(new Dictionary<string, (int, int)>());
    }

    public static class VotingStatsProjectionExtensions
    {
        public static VotingStatsProjection Reduce(this VotingStatsProjection state, object @event)
        {
            state = state ?? VotingStatsProjection.Empty();
            switch (@event)
            {
                case VotingStartedEvent votingStarted:
                    return state.With(
                        votingStarted.Topics.ToDictionary(x => x, _ => (percent: 0, votes: 0)));

                case TopicVotedEvent voted:
                    return state.With(
                        state.Votes.ToDictionary(
                        t => t.Key,
                        t => t.Key == voted.Topic
                                ? (Percent(t.Value.votes + 1), t.Value.votes + 1)
                                : (Percent(t.Value.votes), t.Value.votes)));
                
                case VotingFinishedEvent votingFinished:
                    return state.With(votingFinished.Winner);

                default:
                    return state;
            }

            int Percent(int votes) => (int)Math.Round(votes * 100.00 / (state.Votes.Sum(x => x.Value.votes) + 1));
        }
    }
}