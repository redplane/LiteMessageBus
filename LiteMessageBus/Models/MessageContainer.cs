using System;

namespace LiteMessageBus.Models
{
    public class MessageContainer<T>
    {
        #region Properties

        public DateTime CreatedTime { get; }

        public bool Available { get; set; }

        public object Data { get; }

        #endregion

        #region Constructor

        public MessageContainer(T data, bool available)
        {
            Data = data;
            Available = available;
            CreatedTime = DateTime.UtcNow;
        }

        #endregion

    }
}