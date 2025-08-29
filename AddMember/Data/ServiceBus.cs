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

            // Configuración de la conexión a RabbitMQ
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar la cola (asegura que exista)
            _channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"],
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public async Task SendMessageAsync(string name, string lastname, string birthyear)
        {
            // Crear el cuerpo del mensaje
            var messageBody = $"Name: {name}, Lastname: {lastname}, Birthyear: {birthyear}";
            var body = Encoding.UTF8.GetBytes(messageBody);

            // Publicar el mensaje
            _channel.BasicPublish(exchange: "",
                                routingKey: _configuration["RabbitMQ:QueueName"],
                                basicProperties: null,
                                body: body);

            await Task.CompletedTask; // Simula async para compatibilidad
        }

        // Liberar recursos al finalizar
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}