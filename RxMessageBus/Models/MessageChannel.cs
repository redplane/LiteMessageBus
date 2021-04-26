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

		#region Methods

		public override bool Equals(object obj)
		{
			if (!(obj is MessageChannel messageChannel))
				return false;

			return Name.Equals(messageChannel.Name) && EventName.Equals(messageChannel.EventName);
		}

		protected bool Equals(MessageChannel other)
		{
			return Name == other.Name && EventName == other.EventName;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (EventName != null ? EventName.GetHashCode() : 0);
			}
		}

		#endregion
	}
}