using System.Collections.Generic;

namespace VotingApp.Domain
{
    public class TopicVotedEvent
    {
        public readonly string Topic;

        public TopicVotedEvent(string topic) => Topic = topic;

        public override string ToString() => 
            $"{this.GetType().Name}-{nameof(Topic)}:{Topic}";
    }
}