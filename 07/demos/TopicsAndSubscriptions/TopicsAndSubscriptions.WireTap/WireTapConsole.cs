using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TopicsAndSubscriptions.WireTap
{
    class WireTapConsole
    {
        // Enter a Service Bus connection string.
        private static string ServiceBusConnectionString = "";
        private static string OrdersTopicPath = "Orders";



        static async Task Main(string[] args)
        {
            Console.WriteLine("Wire Tap Console");
            Console.WriteLine("Press enter to activate wire tap");
            Console.ReadLine();

            var subscriptionName = $"wiretap-{ Guid.NewGuid() }";

            var managementClient = new ManagementClient(ServiceBusConnectionString);

            await managementClient.CreateSubscriptionAsync
                (new SubscriptionDescription(OrdersTopicPath, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
            });

            var subscriptionClinet = new SubscriptionClient
                (ServiceBusConnectionString, OrdersTopicPath, subscriptionName);


            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = true
            };

            subscriptionClinet.RegisterMessageHandler(InspectMessageAsync, messageHandlerOptions);
            Console.WriteLine($"Receiving on { subscriptionName }");
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();

            await subscriptionClinet.CloseAsync();
        }

        private static async Task InspectMessageAsync
            (Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received message...");

            Console.WriteLine("Properties");
            Console.WriteLine($"    ContentType             - { message.ContentType }");
            Console.WriteLine($"    CorrelationId           - { message.CorrelationId }");
            Console.WriteLine($"    ExpiresAtUtc            - { message.ExpiresAtUtc }");
            Console.WriteLine($"    Label                   - { message.Label }");
            Console.WriteLine($"    MessageId               - { message.MessageId }");
            Console.WriteLine($"    PartitionKey            - { message.PartitionKey }");
            Console.WriteLine($"    ReplyTo                 - { message.ReplyTo }");
            Console.WriteLine($"    ReplyToSessionId        - { message.ReplyToSessionId }");
            Console.WriteLine($"    ScheduledEnqueueTimeUtc - { message.ScheduledEnqueueTimeUtc }");
            Console.WriteLine($"    SessionId               - { message.SessionId }");
            Console.WriteLine($"    Size                    - { message.Size }");
            Console.WriteLine($"    TimeToLive              - { message.TimeToLive }");
            Console.WriteLine($"    To                      - { message.To }");

            Console.WriteLine("SystemProperties");
            Console.WriteLine($"    EnqueuedTimeUtc - { message.SystemProperties.EnqueuedTimeUtc }");
            Console.WriteLine($"    LockedUntilUtc  - { message.SystemProperties.LockedUntilUtc }");
            Console.WriteLine($"    SequenceNumber  - { message.SystemProperties.SequenceNumber }");

            Console.WriteLine("UserProperties");
            foreach (var property in message.UserProperties)
            {
                Console.WriteLine($"    { property.Key } - { property.Value }");
            }

            Console.WriteLine("Body");
            Console.WriteLine($"{ Encoding.UTF8.GetString(message.Body) }");
            Console.WriteLine();
        }

        private static async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}
