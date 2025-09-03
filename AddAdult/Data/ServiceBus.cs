using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace AddAdult.Data
{
    internal class ServiceBus
    {
        public interface ISubscriptionReceiver
        {
            Task ProcessMessagesAsync(CancellationToken cancellationToken);
            Task StopProcessingAsync();
        }

        public class SubscriptionReceiver : ISubscriptionReceiver
        {
            private readonly IConfiguration _configuration;
            private readonly IServiceProvider _serviceProvider;
            private IConnection? _connection;
            private IChannel? _channel;
            private AsyncEventingBasicConsumer? _consumer;
            private string? _queueName;

            public SubscriptionReceiver(IConfiguration configuration, IServiceProvider serviceProvider)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                if (string.IsNullOrEmpty(_configuration["RabbitMQ:Host"]))
                    throw new InvalidOperationException("RabbitMQ:Host no configurado en appsettings.json");
            }

            public async Task ProcessMessagesAsync(CancellationToken cancellationToken)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                    UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                    Password = _configuration["RabbitMQ:Password"] ?? "guest"
                };

                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(exchange: "usuarios_topic", type: "topic", durable: false);
                var queueResult = await _channel.QueueDeclareAsync();
                _queueName = queueResult.QueueName;
                await _channel.QueueBindAsync(queue: _queueName, exchange: "usuarios_topic", routingKey: "adult");

                _consumer = new AsyncEventingBasicConsumer(_channel);
                _consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received: {message} with routing key: {ea.RoutingKey}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        try
                        {
                            string[] fields = message.Split(',');
                            string name = string.Empty;
                            string lastname = string.Empty;
                            int birthyear = 0;
                            string imageUrl = string.Empty;

                            foreach (string field in fields)
                            {
                                string[] keyValue = field.Trim().Split(':');
                                if (keyValue.Length == 2)
                                {
                                    string key = keyValue[0].Trim();
                                    string value = keyValue[1].Trim();
                                    if (key == "Name") name = value;
                                    else if (key == "Lastname") lastname = value;
                                    else if (key == "Birthyear") birthyear = int.Parse(value);
                                }
                            }

                            imageUrl = $"{name.ToLower()}{lastname.ToLower()}.jpg";

                            var adult = new Adult
                            {
                                Name = name,
                                Lastname = lastname,
                                BirthYear = birthyear,
                                ImageURL = imageUrl
                            };

                            context.Adults.Add(adult);
                            await context.SaveChangesAsync();
                            Console.WriteLine("New adult member has been successfully added");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error processing message: {e.Message}");
                        }

                        if (_channel != null)
                        {
                            await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }


                };

                await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: _consumer);
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }

            public async Task StopProcessingAsync()
            {
                if (_consumer != null && _channel != null)
                {
                    var consumerTags = _consumer.ConsumerTags;
                    if (consumerTags != null)
                    {
                        foreach (var consumerTag in consumerTags)
                        {
                            await _channel.BasicCancelAsync(consumerTag);
                        }
                    }
                    await _channel.CloseAsync();
                }
                if (_connection != null)
                {
                    await _connection.CloseAsync();
                }
            }
        }
    }
}