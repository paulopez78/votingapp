using System;

namespace VotingApp.Domain
{
    public class VotingStartedEvent
    {
        public VotingStartedEvent(Guid votingId, params string[] topics)
        {
            VotingId = votingId;
            Topics = topics;
        }

        public Guid VotingId { get; }

        public string[] Topics { get; }

        public override string ToString() =>
            $"{this.GetType().Name}-{nameof(Topics)}:{string.Join(",", Topics)}";
    }
}