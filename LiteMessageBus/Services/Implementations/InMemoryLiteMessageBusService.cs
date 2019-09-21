using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LiteMessageBus.Models;
using LiteMessageBus.Services.Interfaces;

namespace LiteMessageBus.Services.Implementations
{
    public class InMemoryRxMessageBusService : IRxMessageBusService
    {
        #region Properties

        /// <summary>
        /// Chanel event manager.
        /// </summary>
        private readonly ConcurrentDictionary<MessageChannel, ReplaySubject<MessageContainer<object>>>
            _channelManager;

        /// <summary>
        /// Channel initialization manager.
        /// </summary>
        private readonly ConcurrentDictionary<MessageChannel, ReplaySubject<AddedChannelEvent>> _channelInitializationManager;

        #endregion

        #region Constructor

        public InMemoryRxMessageBusService()
        {
            _channelInitializationManager = new ConcurrentDictionary<MessageChannel, ReplaySubject<AddedChannelEvent>>();
            _channelManager = new ConcurrentDictionary<MessageChannel, ReplaySubject<MessageContainer<object>>>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        public void AddMessageChannel<T>(string channelName, string eventName)
        {
            LoadMessageChannel(channelName, eventName, true);;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IObservable<T> HookMessageChannel<T>(string channelName, string eventName)
        {
            return HookChannelInitialization(channelName, eventName)
                .Select(x =>
                {
                    return LoadMessageChannel(channelName, eventName, false)
                        .Where(messageContainer =>
                            {
                                return (messageContainer != null && messageContainer.Available &&
                                        messageContainer.Data is T);
                            })
                        .Select(messageContainer => (T)messageContainer.Data);
                })
                .Switch();

        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IObservable<AddedChannelEvent> HookChannelInitialization(string channelName, string eventName)
        {
            var channelInitializationEventEmitter = _channelInitializationManager.GetOrAdd(new MessageChannel(channelName, eventName), new ReplaySubject<AddedChannelEvent>());
            return channelInitializationEventEmitter;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddMessage<T>(string channelName, string eventName, T data)
        {
            var messageContainer = new MessageContainer<object>(data, true);
            var channelMessageEmitter = LoadMessageChannel(channelName, eventName, true);
            if (channelMessageEmitter == null)
                return;

            channelMessageEmitter.OnNext(messageContainer);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        public void DeleteMessage(string channelName, string eventName)
        {
            var messageContainer = new MessageContainer<object>(null, false);
            var channelMessageEmitter = LoadMessageChannel(channelName, eventName, true);

            // Emit the blank message to channel.
            channelMessageEmitter.OnNext(messageContainer);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void DeleteMessages()
        {
            var keys = _channelManager.Keys;
            foreach (var key in keys)
            {
                if (!_channelManager.TryGetValue(key, out var channelMessageEmitter))
                    continue;

                var messageContainer = new MessageContainer<object>(null, false);
                channelMessageEmitter.OnNext(messageContainer);
            }
        }

        /// <summary>
        /// Load message channel using channel name and event name.
        /// Specifying auto create will trigger channel creation if it is not available.
        /// Auto create option can cause concurrent issue, such as parent channel can be replaced by child component.
        /// Therefore, it should be used wisely.
        /// </summary>
        protected internal ReplaySubject<MessageContainer<object>> LoadMessageChannel(string channelName, string eventName, bool autoCreate = false)
        {
            var hasMessageEmitterAdded = false;
            if (!_channelManager.TryGetValue(new MessageChannel(channelName, eventName), out var channelEventEmitter))
            {
                if (!autoCreate)
                    return null;

                channelEventEmitter = new ReplaySubject<MessageContainer<object>>(1);
                if (!_channelManager.TryAdd(new MessageChannel(channelName, eventName), channelEventEmitter))
                    throw new Exception($"Cannot add channel {channelName} and event name {eventName}");

                hasMessageEmitterAdded = true;
            }

            // Channel has been added before.
            if (!hasMessageEmitterAdded)
                return channelEventEmitter;

            // Raise an event about message channel creation if it has been newly added.
            if (!_channelInitializationManager.TryGetValue(new MessageChannel(channelName, eventName),
                out var channelInitializationEventEmitter))
            {
                channelInitializationEventEmitter = new ReplaySubject<AddedChannelEvent>(1);
                _channelInitializationManager.TryAdd(new MessageChannel(channelName, eventName), channelInitializationEventEmitter);
            }

            channelInitializationEventEmitter.OnNext(new AddedChannelEvent(channelName, eventName));
            return channelEventEmitter;
        }


        #endregion
    }
}
