using System;
using LiteMessageBus.Models;

namespace LiteMessageBus.Services.Interfaces
{
    public interface IRxMessageBusService
    {
        #region Methods

        /// <summary>
        ///     Add a message channel to message bus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        void AddMessageChannel<T>(string channelName, string eventName);

        /// <summary>
        ///     Hook message event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        IObservable<T> HookMessageChannel<T>(string channelName, string eventName);

        /// <summary>
        /// Raised when channel is added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        IObservable<AddedChannelEvent> HookChannelInitialization(string channelName, string eventName);

        /// <summary>
        /// Add message to specific event in specific channel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        void AddMessage<T>(string channelName, string eventName, T data);

        /// <summary>
        /// Delete message that has been sent from a specific channel and event.
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        void DeleteMessage(string channelName, string eventName);

        /// <summary>
        /// Delete messages from every channel.
        /// </summary>
        void DeleteMessages();

        #endregion
    }
}