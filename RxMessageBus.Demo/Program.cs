using System;
using LiteMessageBus.Services.Implementations;
using RxMessageBus.Demo.Constants;

namespace RxMessageBus.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started");
            var inMemoryMessageBus = new InMemoryLiteMessageBusService();
            inMemoryMessageBus.HookMessageChannel<string>(MessageChannelConstants.Ui, MessageEventConstants.SendMessage)
                .Subscribe(message =>
                {
                    Console.WriteLine(
                        $"[IN-MEMORY] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Message received: {message}");
                });

            inMemoryMessageBus.AddMessage(MessageChannelConstants.Ui, MessageEventConstants.SendMessage, "Hello world");
            Console.ReadLine();
        }
    }
}