namespace LiteMessageBus.Models
{
    public class MessageChannelOptions
    {
        #region Properties

        public double MessageLifetimeInSeconds { get; }

        #endregion

        #region Constructor

        public MessageChannelOptions(double messageLifetimeInSeconds)
        {
            MessageLifetimeInSeconds = messageLifetimeInSeconds;
        }

        #endregion
    }
}