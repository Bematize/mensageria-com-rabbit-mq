using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var fila = "BematizeEmail";
            /* Se a aplicação estiver no mesmo servidor do Rabbit, o hostname pode ser informado como 
                "localhost" e não será necessário usuário e senha. Por padrão será o usuário "guest".
                A menos que deseje acessar usando outro usuário. */

            var factory = new ConnectionFactory()
            {
                HostName = "emu.rmq.cloudamqp.com",
                UserName = "kcdrbgsw",
                Password = "GQNUnn1SZrONcExUvNQRYWgcVXGBYj3l",
                VirtualHost = "kcdrbgsw"
                // Ou
                //Uri = new Uri("amqp://kcdrbgsw:GQNUnn1SZrONcExUvNQRYWgcVXGBYj3l@emu.rmq.cloudamqp.com/kcdrbgsw")
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: fila, durable: true, exclusive: false, autoDelete: false, arguments: null);

                Console.WriteLine(" [*] Esperando por mensagens.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Mensagem {0}", message);
                };
                channel.BasicConsume(queue: fila, autoAck: true, consumer: consumer);

                Console.WriteLine(" Pressione [enter] para sair.");
                Console.ReadLine();
            }
        }
    }
}
