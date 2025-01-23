using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace WebApplication1;

public class OrderConsumerService(ConnectionFactoryBuilder connectionFactoryBuilder) : BackgroundService
{
    private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(5);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var _channel = await CreateConnection(connectionFactoryBuilder);
            _ = await _channel.QueueDeclareAsync(queue: "orders",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

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
            await Task.Delay(_intervalo, stoppingToken);
        }
    }

    private static async Task<IChannel> CreateConnection(ConnectionFactoryBuilder connectionFactoryBuilder)
    {
        var factory = connectionFactoryBuilder.Build();
        var conn = await factory.CreateConnectionAsync();
        return await conn.CreateChannelAsync();
    }

    public override void Dispose() => base.Dispose();
}
