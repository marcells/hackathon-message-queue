using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure
{
    public static class QueueHelper
    {
        public static void ListenOn(IHandler handler, string @namespace, IList<string> topicPatterns)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "hello", type: "topic");

            var queueName = channel.QueueDeclare().QueueName;

            foreach(var topicPattern in topicPatterns)
            {
                channel.QueueBind(queueName, exchange: "hello", routingKey: topicPattern);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var handlerType = handler.GetType();

                var jsonified = Encoding.UTF8.GetString(ea.Body);

                var type = handlerType.Assembly.GetType($"{@namespace}.{ea.BasicProperties.Type}");
                var message = JsonConvert.DeserializeObject(jsonified, type);

                var handlerMethod = handlerType
                    .GetMethods()
                    .Where(x => x.Name == "Handle")
                    .Where(x => x.GetParameters().FirstOrDefault(y => y.ParameterType == type) != null)
                    .SingleOrDefault();

                if (handlerMethod != null)
                {
                    handlerMethod.Invoke(handler, new[] { message });
                }
                else
                {
                    handler.Handle(message);
                }

                Console.WriteLine("Pure json: {0}", jsonified);
                Console.WriteLine("--------------------------");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Marcell ist schuld (started).");
        }

        public static void Publish(object message, string topic)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "hello", type: "topic");

                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.Persistent = true;
                    basicProperties.Type = message.GetType().Name;
                    var jsonified = JsonConvert.SerializeObject(message);
                    var customerBuffer = Encoding.UTF8.GetBytes(jsonified);

                    channel.BasicPublish(exchange: "hello",
                                        routingKey: topic,
                                        basicProperties: basicProperties,
                                        body: customerBuffer);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
