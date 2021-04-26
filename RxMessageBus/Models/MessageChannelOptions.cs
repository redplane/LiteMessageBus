namespace LiteMessageBus.Models
{
    public class MessageChannelOptions
    {
        #region Properties

        /// <summary>
        /// Life time which message is valid (in seconds)
        /// </summary>
        public double? LifeTime { get; }

        #endregion

        #region Constructor

        public MessageChannelOptions(double lifeTime)
        {
            LifeTime = lifeTime;
        }

        #endregion
    }
}