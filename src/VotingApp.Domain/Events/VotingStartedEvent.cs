namespace VotingApp.Domain
{
    public class VotingStartedEvent
    {
        public VotingStartedEvent(params string[] topics) => Topics = topics;

        public string[] Topics { get; }

        public override string ToString() => 
            $"{this.GetType().Name}-{nameof(Topics)}:{string.Join(",",Topics)}";
    }
}