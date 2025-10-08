using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AddMember.Data
{
    public interface IServiceBus
    {
        Task SendMessageAsync(string name, string lastname, string birthyear);
    }

    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("usuarios_topic", ExchangeType.Topic, durable: true);
            _channel.QueueDeclare("member_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind("member_queue", "usuarios_topic", "member");
        }

        public async Task SendMessageAsync(string name, string lastname, string birthyear)
        {
            var messageBody = $"Name: {name}, Lastname: {lastname}, Birthyear: {birthyear}";
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublish(
                exchange: "usuarios_topic",
                routingKey: "member",
                basicProperties: null,
                body: body
            );

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
