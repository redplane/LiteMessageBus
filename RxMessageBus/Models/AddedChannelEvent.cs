using System;

namespace LiteMessageBus.Models
{
    public class AddedChannelEvent
    {
        #region Properties

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string ChannelName { get; private set; }

        // ReSharper disable once UnusedMember.Global
        public string EventName { get; private set; }

        #endregion

        #region Constructor

        public AddedChannelEvent(string channelName, string eventName = default)
        {
            if (string.IsNullOrWhiteSpace(channelName))
                throw new Exception($"{nameof(channelName)} cannot be blank");
            ChannelName = channelName;
            EventName = eventName;
        }

        #endregion
    }
}