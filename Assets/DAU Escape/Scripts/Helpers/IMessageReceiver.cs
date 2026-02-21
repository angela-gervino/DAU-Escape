using System;

namespace DAUEscape
{
    public enum MessageType
    {
        DAMAGED,
        DEAD
    }
    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type);
    }
}
