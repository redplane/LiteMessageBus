using System;
using LiteMessageBus.Services.Implementations;
using RxMessageBusDemo.Constants;

namespace RxMessageBusDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var inMemoryMessageBus = new InMemoryRxMessageBusService();
            inMemoryMessageBus.HookMessageChannel<string>(MessageChannelConstants.Ui, MessageEventConstants.SendMessage)
                .Subscribe(message =>
                    Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Message received: {message}"));

            inMemoryMessageBus.AddMessage(MessageChannelConstants.Ui, MessageEventConstants.SendMessage, "Hello world");
        }
    }
}
