using System;
using LiteMessageBus.Services.Implementations;
using LiteMessageBusDemo.Constants;
using LiteMessageBusDemo.Services;

namespace LiteMessageBusDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started");
            var inMemoryMessageBus = new InMemoryLiteMessageBusService();
            inMemoryMessageBus.HookMessageChannel<string>(MessageChannelConstants.Ui, MessageEventConstants.SendMessage)
                .Subscribe(message =>
                    Console.WriteLine(
                        $"[IN-MEMORY] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Message received: {message}"));

            const string appId = "875585";
            const string appKey = "7924946bbbd2fc014135";
            const string appSecret = "a6c2c1f9ae29637d995a";

            var pusherServerOptions = new PusherServer.PusherOptions();
            pusherServerOptions.Cluster = "ap1";
            pusherServerOptions.Encrypted = true;

            var pusherClientOptions = new PusherClient.PusherOptions();
            pusherClientOptions.Cluster = "ap1";
            pusherClientOptions.Encrypted = true;

            var broadcaster = new PusherServer.Pusher(appId,
                appKey, appSecret, pusherServerOptions);

            var recipient = new PusherClient.Pusher(appId, pusherClientOptions);

            // Pusher message bus initialization.
            var pusherMessageBus = new PusherLiteMessageBusService(broadcaster, recipient);
            pusherMessageBus
                .HookMessageChannel<string>(MessageChannelConstants.Ui, MessageEventConstants.SendMessage)
                .Subscribe(message =>
                {
                    Console.WriteLine(
                        $"[PUSHER] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - Message received: {message}");
                });

            inMemoryMessageBus.AddMessage(MessageChannelConstants.Ui, MessageEventConstants.SendMessage, "Hello world");
            pusherMessageBus.AddMessage(MessageChannelConstants.Ui, MessageEventConstants.SendMessage, "Hello pusher");
            Console.WriteLine("App started");
        }
    }
}