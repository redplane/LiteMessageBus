using System.Reactive.Subjects;
using LiteMessageBus.Models;
using PusherClient;

namespace LiteMessageBusDemo.Models
{
    public class MessageChannelOption
    {
        #region Properties

        public ReplaySubject<MessageContainer<object>> InnerMessageSender { get; }

        public PusherClient.Channel PusherChannel { get; }

        #endregion

        #region Constructor

        public MessageChannelOption(ReplaySubject<MessageContainer<object>> innerMessageSender, Channel pusherChannel)
        {
            InnerMessageSender = innerMessageSender;
            PusherChannel = pusherChannel;
        }

        #endregion
    }
}