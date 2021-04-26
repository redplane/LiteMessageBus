using System;
using LiteMessageBus.Models;

namespace LiteMessageBus.Services.Interfaces
{
    [Obsolete("This will be removed in the next version, please use IRxMessageBusService instead.")]
    public interface ILiteMessageBusService
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
        /// Add a message channel to message bus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelEvent"></param>
        void AddTypedMessageChannel<T>(TypedChannelEvent<T> channelEvent);

        /// <summary>
        ///     Hook message event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        IObservable<T> HookMessageChannel<T>(string channelName, string eventName);

        /// <summary>
        /// Hook typed message event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelEvent"></param>
        /// <returns></returns>
        IObservable<T> HookTypedMessageChannel<T>(TypedChannelEvent<T> channelEvent);

        /// <summary>
        /// Add message to specific event in specific channel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        void AddMessage<T>(string channelName, string eventName, T data);

        /// <summary>
        /// Add typed message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelEvent"></param>
        /// <param name="data"></param>
        void AddTypedMessage<T>(TypedChannelEvent<T> channelEvent, T data);

        /// <summary>
        /// Delete message that has been sent from a specific channel and event.
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        void DeleteMessage(string channelName, string eventName);

        /// <summary>
        /// Delete message that has been sent to a specific channel & event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelEvent"></param>
        void DeleteTypedMessage<T>(TypedChannelEvent<T> channelEvent);

        /// <summary>
        /// Delete messages from every channel.
        /// </summary>
        void DeleteMessages();

        #endregion
    }
}