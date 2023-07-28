using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _chanel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _chanel = _connection.CreateModel();
                _chanel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);

                _connection.ConnectionShutdown += _connection_ConnectionShutdown;

                Console.WriteLine("---> Connect to the message bus");

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto published)
        {
            string message = JsonSerializer.Serialize(published);
            if(_connection.IsOpen)
            {
                Console.WriteLine("--> Connection is open");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> Connection is off");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _chanel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine("--> We sent the message");
        }

        public void Dispose()
        {
            Console.WriteLine("---> Chanel dispose");
            if(_connection.IsOpen)
            {
                _chanel.Close();
                _connection.Close();
            }
        }

        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

       
    }
}
