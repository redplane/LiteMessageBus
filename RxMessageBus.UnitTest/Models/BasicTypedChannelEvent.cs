using LiteMessageBus.Models;

namespace RxMessageBus.UnitTest.Models
{
	public class BasicTypedChannelEvent : TypedChannelEvent<string>
	{
		#region Constructor

		public BasicTypedChannelEvent() : base("basic-channel-name", "basic-event-name")
		{
		}

		#endregion
	}
}