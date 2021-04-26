using System;
using System.Reactive.Linq;
using LiteMessageBus.Services.Implementations;
using NUnit.Framework;
namespace LiteMessageBus.UnitTest.Services
{
    [TestFixture]
    public class InMemoryLiteMessageBusServiceTests
    {
        #region Tests

        [Test]
        public void AddMessageChannel_WhenCalled_MethodRunsSuccessfully()
        {
            // Arrange
            const string channelName = "chanel-01";
            const string channelEvent = "event-01";
            
            // Initialize message bus service instance.
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            
            // Add message channel to message bus.
            inMemoryLiteMessageBusService.AddMessageChannel<string>(channelName, channelEvent);

            Assert.Pass();
        }

        [Test]
        public void HookMessageChannel_WhenCalled_CanHookToAvailableAndUnavailableChannel()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            var hooker = inMemoryLiteMessageBusService.HookMessageChannel<string>(channelName, channelEvent);
            Assert.NotNull(hooker);
        }

        [Test]
        public void AddMessage_WhenCalled_CanAddMessageToChannel()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            const string message = "hello world";
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            inMemoryLiteMessageBusService.AddMessage<string>(channelName, channelEvent, message);
            
            // As no exception occurs, the method runs successfully.
            Assert.Pass();
        }

        [Test]
        public void AddMessage_WhenCalled_CanHookAddedMessage()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            const string message = "hello world";

            var loadedMessage = string.Empty;
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            
            // Hook to message channel first.
            var hooker = inMemoryLiteMessageBusService.HookMessageChannel<string>(channelName, channelEvent);
            hooker.Subscribe(innerMessage => loadedMessage = innerMessage);
            
            // Add message to channel.
            inMemoryLiteMessageBusService.AddMessage(channelName, channelEvent, message);
            
            Assert.That(loadedMessage, Is.EqualTo(message).After(2000, 500));
        }

        [Test]
        public void DeleteMessage_WhenCalled_CannotGetDeletedMessage()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            const string message = "hello world";

            var actualMessage = string.Empty;
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            inMemoryLiteMessageBusService.AddMessage(channelName, channelEvent, message);
            
            // Delete the message.
            inMemoryLiteMessageBusService.DeleteMessage(channelName, channelEvent);
            
            // Hook to the message channel.
            var hooker = inMemoryLiteMessageBusService.HookMessageChannel<string>(channelName, channelEvent);
            hooker.Subscribe(m => actualMessage = m);
            
            Assert.That(actualMessage, Is.Not.EqualTo(message).After(2000, 500));
        }

        [Test]
        public void DeleteMessage_WhenCalled_CannotSubscribeToDeletedMessageChannel()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            const string message = "hello world";

            var hasChannelSubscribed = false;
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            inMemoryLiteMessageBusService.AddMessage(channelName, channelEvent, message);
            
            // Delete the message.
            inMemoryLiteMessageBusService.DeleteMessage(channelName, channelEvent);
            
            // Hook to the message channel.
            var hooker = inMemoryLiteMessageBusService.HookMessageChannel<string>(channelName, channelEvent);
            hooker.Subscribe(_ => hasChannelSubscribed = true);
            
            Assert.That(hasChannelSubscribed, Is.Not.True.After(2000, 500));
        }

        [Test]
        public void DeleteMessages_WhenCalled_CannotSubscribeToChannelsDeletedMessages()
        {
            const string channelName = "channel-01";
            const string channelEvent = "event-01";
            const string message = "hello world";

            var hasChannelSubscribed = false;
            
            var inMemoryLiteMessageBusService = new InMemoryLiteMessageBusService();
            inMemoryLiteMessageBusService.AddMessage(channelName, channelEvent, message);
            
            // Delete the message.
            inMemoryLiteMessageBusService.DeleteMessages();
            
            // Hook to the message channel.
            var hooker = inMemoryLiteMessageBusService.HookMessageChannel<string>(channelName, channelEvent);
            hooker.Subscribe(_ => hasChannelSubscribed = true);
            
            Assert.That(hasChannelSubscribed, Is.Not.True.After(2000, 500));
        }
        
        #endregion
    }
}