namespace LiteMessageBus.Models
{
	public abstract class TypedChannelEvent<T>
	{
		#region Properties

		// ReSharper disable once UnusedAutoPropertyAccessor.Local
		public string ChannelName { get; private set; }

		// ReSharper disable once UnusedMember.Global
		public string EventName { get; private set; }

		#endregion

		#region Constructor

		protected TypedChannelEvent(string channelName, string eventName)
		{
			ChannelName = channelName;
			EventName = eventName;
		}

		#endregion
	}
}