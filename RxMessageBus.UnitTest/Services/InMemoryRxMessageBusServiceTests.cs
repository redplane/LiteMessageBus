using System;
using System.Collections.Generic;
using LiteMessageBus.Services.Implementations;
using LiteMessageBus.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RxMessageBus.UnitTest.Models;

namespace RxMessageBus.UnitTest.Services
{
	[TestFixture]
	public class InMemoryRxMessageBusServiceTests
	{
		#region Properties

		private readonly LinkedList<IDisposable> _subscriptions;

		private readonly IServiceCollection _services;

		#endregion

		#region Constructor

		public InMemoryRxMessageBusServiceTests()
		{
			_subscriptions = new LinkedList<IDisposable>();
			_services = new ServiceCollection();
		}

		#endregion

		#region Installation

		/// <summary>
		/// Run before every test.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			_services.AddScoped<IRxMessageBus, InMemoryRxMessageService>();
		}

		/// <summary>
		/// Run after every test.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			foreach (var subscription in _subscriptions)
				subscription.Dispose();

			_services.Clear();
		}

		#endregion

		#region Methods

		[Test]
		public void AddTypedMessage_WhenCalled_CanHookAddedMessage()
		{
			const string message = "hello world";

			var loadedMessage = string.Empty;

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();

			// Hook to message channel first.
			var observable = messageBusService.HookTypedMessageChannel(new BasicTypedChannelEvent());
			var subscription = observable.Subscribe(innerMessage => loadedMessage = innerMessage);
			_subscriptions.AddLast(subscription);

			// Add message to channel.
			messageBusService.AddTypedMessage(new BasicTypedChannelEvent(), message);
			Assert.That(loadedMessage, Is.EqualTo(message).After(2000, 500));
		}

		[Test]
		public void DeleteTypedMessage_WhenCalled_CannotGetDeletedMessage()
		{
			const string message = "hello world";

			var actualMessage = string.Empty;

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();
			messageBusService.AddTypedMessage(new BasicTypedChannelEvent(), message);

			// Delete the message.
			messageBusService.DeleteTypedMessage(new BasicTypedChannelEvent());

			// Hook to the message channel.
			var observable = messageBusService.HookTypedMessageChannel(new BasicTypedChannelEvent());
			var subscription = observable.Subscribe(m => actualMessage = m);
			_subscriptions.AddLast(subscription);

			Assert.That(actualMessage, Is.Not.EqualTo(message).After(2000, 500));
		}

		[Test]
		public void DeleteMessage_WhenCalled_CannotSubscribeToDeletedMessageChannel()
		{
			const string message = "hello world";
			var hasChannelSubscribed = false;

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();
			messageBusService.AddTypedMessage(new BasicTypedChannelEvent(), message);

			// Delete the message.
			messageBusService.DeleteTypedMessage(new BasicTypedChannelEvent());

			// Hook to the message channel.
			var observable = messageBusService.HookTypedMessageChannel(new BasicTypedChannelEvent());
			var subscription = observable.Subscribe(_ => hasChannelSubscribed = true);
			_subscriptions.AddLast(subscription);

			Assert.That(hasChannelSubscribed, Is.Not.True.After(2000, 500));
		}

		[Test]
		public void DeleteMessages_WhenCalled_CannotSubscribeToChannelsDeletedMessages()
		{
			const string message = "hello world";
			var hasChannelSubscribed = false;

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();
			messageBusService.AddTypedMessage(new BasicTypedChannelEvent(), message);

			// Delete the message.
			messageBusService.DeleteMessages();

			// Hook to the message channel.
			var hooker = messageBusService.HookTypedMessageChannel<string>(new BasicTypedChannelEvent());
			hooker.Subscribe(_ => hasChannelSubscribed = true);

			Assert.That(hasChannelSubscribed, Is.Not.True.After(2000, 500));
		}

		[Test]
		public void AddMessage_Expects_HookMessageChannelCanReceiveMessage()
		{
			const string message = "hello world";
			var basicTypedChannelEvent = new BasicTypedChannelEvent();

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();
			
			// Hook the message channel event.
			var subscription = messageBusService.HookTypedMessageChannel(basicTypedChannelEvent)
				.Subscribe(
					incomingMessage => message.Equals(incomingMessage, StringComparison.CurrentCultureIgnoreCase));
			_subscriptions.AddLast(subscription);
			
			messageBusService.AddMessage(basicTypedChannelEvent.ChannelName, basicTypedChannelEvent.EventName, message);
		}

		[Test]
		public void AddTypedMessage_Expects_HookTypedMessageChannelCanReceiveMessage()
		{
			const string message = "hello world";
			var basicTypedChannelEvent = new BasicTypedChannelEvent();

			var serviceProvider = _services.BuildServiceProvider();
			var messageBusService = serviceProvider.GetService<IRxMessageBus>();

			// Hook the message channel event.
			var subscription = messageBusService
				.HookMessageChannel<string>(basicTypedChannelEvent.ChannelName, basicTypedChannelEvent.EventName)
				.Subscribe(
					incomingMessage => message.Equals(incomingMessage, StringComparison.CurrentCultureIgnoreCase));
			_subscriptions.AddLast(subscription);

			messageBusService.AddTypedMessage(basicTypedChannelEvent, message);
		}

		#endregion
	}
}