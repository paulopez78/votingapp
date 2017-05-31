using System;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Domain;
using Xunit;

namespace VotingApp.Tests
{
    public class VotingAggregateTests
    {
        [Fact]
        public void Given_Two_Topics_When_Starting_Voting_Then_VotingStartedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();
            
            // Act
            sut.Start("C#", "F#");

            // Assert
            var @event = sut.GetPendingEvents().OfType<VotingStartedEvent>().FirstOrDefault();
            Assert.NotNull(@event);
            Assert.Equal(@event.Topics, new string[] { "C#", "F#" });
        }

        [Fact]
        public void Given_Started_Voting_When_Vote_For_Valid_Topic_Then_TopicVotedEventCreated()
        {
            // Arrange
            var sut = new VotingAggregate();            
            sut.Start("C#", "F#");

            // Act
            sut.Vote("C#");

            // Assert
            var @event = sut.GetPendingEvents().OfType<TopicVotedEvent>().FirstOrDefault();
            Assert.NotNull(@event);
            Assert.Equal(@event.Topic, "C#");
        }

        [Fact]
        public void Given_Started_Voting_With_Votes_When_Finish_Then_VotingFinishedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();            
            sut.Start("C#", "F#");
            sut.Vote("C#");       

            // Act
            sut.Finish();

            // Assert
            var @event = sut.GetPendingEvents().OfType<VotingFinishedEvent>().FirstOrDefault();
            Assert.NotNull(@event);
            Assert.Equal(@event.Winner, "C#");
        }

        [Fact]
        public void Given_Started_Voting_With_Votes_When_Finish_With_Same_Votes_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();            
            sut.Start("C#", "F#");
            sut.Vote("C#");
            sut.Vote("F#");

            // Act
            Action result = () => sut.Finish();

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_Started_Voting_When_Vote_For_Invalid_Topic_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();            
            sut.Start("C#", "F#");

            // Act
            Action result = () => sut.Vote("VB .NET");

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_Finished_Voting_When_Vote_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();            
            sut.Start("C#", "F#");
            sut.Vote("C#");
            sut.Finish();

            // Act
            Action result = () => sut.Vote("C#");

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(result);
        }
    }
}
