using Amido.Stacks.Messaging.Azure.ServiceBus.Tests.IntegrationTests.Steps;
using AutoFixture.Xunit2;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.IntegrationTests.Stories
{
    public class PublishingEventTests
    {
        [Trait("Type", "IApplicationEvent on Azure Service Bus")]
        [Trait("Category", "Validated pass")]
        [Theory(Skip = "due to missing queue used by pipeline to run tests")]
        //[Theory]
        [AutoData]
        public void PublishAndHandlingApplicationEvent(PublishEventFixtures fixture)
        {
            this.Given(_ => fixture.TheCorrectApplicationEventIsSentToTheTopic())
                .And(_ => fixture.TheTopicSenderHealthCheckPass())
                .And(_ => fixture.TheHostIsRunning())
                .And(_ => fixture.WaitFor3Seconds())
                .Then(_ => fixture.TheApplicationEventIsHandledInTheHandler())
                .BDDfy();
        }

        [Trait("Type", "IEvent on Azure Service Bus")]
        [Trait("Category", "Validated pass")]
        [Theory(Skip = "due to missing queue used by pipeline to run tests")]
        //[Theory]
        [AutoData]
        public void PublishAndHandlingEvent(PublishEventFixtures fixture)
        {
            this.Given(_ => fixture.TheCorrectEventIsSentToTheTopic())
                .And(_ => fixture.TheTopicSenderHealthCheckPass())
                .And(_ => fixture.TheHostIsRunning())
                .And(_ => fixture.WaitFor3Seconds())
                .Then(_ => fixture.TheEventIsHandledInTheHandler())
                .BDDfy();
        }

        [Trait("Type", "IEvent on Azure Service Bus")]
        [Trait("Category", "Validated pass")]
        [Theory(Skip = "due to missing queue used by pipeline to run tests")]
        //[Theory]
        [AutoData]
        public void PublishAndHandlingBatchedEvents(PublishEventFixtures fixture)
        {
            this.Given(_ => fixture.TheCorrectBatchOfEventsIsSentToTheTopic())
                .And(_ => fixture.TheTopicSenderHealthCheckPass())
                .And(_ => fixture.TheHostIsRunning())
                .And(_ => fixture.WaitFor3Seconds())
                .Then(_ => fixture.TheBatchOfEventsIsHandledInTheHandler())
                .BDDfy();
        }

        [Trait("Type", "MessageEnvelope on Azure Service Bus")]
        [Trait("Category", "Validated pass")]
        [Theory(Skip = "due to missing queue used by pipeline to run tests")]
        //[Theory]
        [AutoData]
        public void PublishAndHandlingMessageEnvelope(PublishEventFixtures fixture)
        {
            this.Given(_ => fixture.TheCorrectMessageEnvelopeIsSentToTheTopic())
                .And(_ => fixture.TheTopicSenderHealthCheckPass())
                .And(_ => fixture.ReadMessageEnvelopeFromTopic())
                .And(_ => fixture.WaitFor3Seconds())
                .Then(_ => fixture.TheMessageIsHandledInTheHandler())
                .BDDfy();
        }
    }
}
