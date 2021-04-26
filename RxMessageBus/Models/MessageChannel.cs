using System;
using System.Collections.Generic;

namespace LiteMessageBus.Models
{
    public class MessageChannel
    {
        #region Properties

        /// <summary>
        /// Name of channel.
        /// </summary>public
        public string Name { get; }

        /// <summary>public
        /// Name of event.
        /// </summary>
        public string EventName { get; }

        #endregion

        #region Constructor

        public MessageChannel(string name, string eventName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Name cannot be null either blank.");

            if (string.IsNullOrWhiteSpace(eventName))
                throw new Exception("Event name cannot be null either blank.");


            Name = name.ToLower().Trim();
            EventName = eventName.ToLower().Trim();
        }

        #endregion
    }
}