using System;
using LiteMessageBus.Services.Implementations;
using LiteMessageBusDemo.Constants;

namespace LiteMessageBusDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var inMemoryMessageBus = new InMemoryLiteMessageBusService();
            inMemoryMessageBus.HookMessageChannel<string>(MessageChannelConstants.Ui, MessageEventConstants.SendMessage)
                .Subscribe(message =>
                    Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Message received: {message}"));

            inMemoryMessageBus.AddMessage(MessageChannelConstants.Ui, MessageEventConstants.SendMessage, "Hello world");
        }
    }
}
