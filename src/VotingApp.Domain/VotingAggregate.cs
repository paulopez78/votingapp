using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.Aggregate;

namespace VotingApp.Domain
{
    public class VotingAggregate : AggregateRoot
    {
        public VotingAggregate()
        {
        }

        public VotingAggregate(Guid id) : base(id)
        {
        }

        public VotingProjection State { get; private set; }

        public void Start(params string[] topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics));
            if (topics.Length < 2) throw new DomainException($"Provide at least 2 topics for starting the voting");
            if (topics.Distinct().Count() != topics.Count()) throw new DomainException("Duplicated topics are not allowed");

            RaiseEvent(new VotingStartedEvent(Id, topics));
        }

        public void Vote(string topic)
        {
            if (!string.IsNullOrEmpty(State.Winner)) throw new DomainException($"The voting is over winner was {State.Winner}");
            if (!State.Topics.ContainsKey(topic)) throw new DomainException($"Not valid topic {topic}");
            RaiseEvent(new TopicVotedEvent(topic));
        }

        public void Finish()
        {
            var winners = State.Topics.Where(x => x.Value == State.Topics.Max(t => t.Value));
            if (winners.Count() > 1) throw new DomainException("Can't finish voting with same amount of votes");
            RaiseEvent(new VotingFinishedEvent(winners.First().Key));
        }

        public void Apply(object @event) => State = State.Reduce(@event);
    }
}
