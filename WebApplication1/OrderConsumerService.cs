using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace WebApplication1;

public class OrderConsumerService(ConnectionFactoryBuilder connectionFactoryBuilder) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = connectionFactoryBuilder.Build();
        using IConnection conn = await factory.CreateConnectionAsync(stoppingToken);
        using IChannel _channel = await conn.CreateChannelAsync(cancellationToken: stoppingToken);
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
