namespace VotingApp.Domain
{
    public class VotingFinishedEvent
    {
        public string Winner { get; }

        public VotingFinishedEvent(string winner) => Winner = winner;

        public override string ToString() => 
            $"{this.GetType().Name}-{nameof(Winner)}:{Winner}";
    }
}