using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace WebApplication1;

public class OrderConsumerService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "172.17.0.3",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            RequestedConnectionTimeout = TimeSpan.FromSeconds(10),  // Timeout mais longo
            AutomaticRecoveryEnabled = true,  // Habilita reconexão automática
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
        };
        using IConnection conn = await factory.CreateConnectionAsync();
        using IChannel _channel = await conn.CreateChannelAsync();
        _ = await _channel.QueueDeclareAsync(queue: "orders", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (ch, ea) =>
         {
             var body = ea.Body.ToArray();
             var message = Encoding.UTF8.GetString(body);
             var order = JsonSerializer.Deserialize<OrderRequest>(message);

             Console.WriteLine($"Pedido recebido: Id={order.OrderId}, Produto={order.ProductName}, Quantidade={order.Quantity}");
             // Lógica de processamento do pedido
             return Task.CompletedTask;
         };

        var consumerATag = await _channel.BasicConsumeAsync(queue: "orders", autoAck: true, consumer: consumer);
    }

    public override void Dispose() => base.Dispose();
}
