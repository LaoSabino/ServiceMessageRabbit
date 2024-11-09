using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace WebApplication1;

public class OrderService : IOrderService
{
    public async void SendOrder(OrderRequest order)
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
        await _channel.QueueDeclareAsync(queue: "orders", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var message = JsonSerializer.Serialize(order);
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties
        {
            ContentType = "text/plain",
            DeliveryMode = DeliveryModes.Persistent,
            Headers = new Dictionary<string, object?>()
        };
        props.Headers.Add("Servico", 1000);

        await _channel.BasicPublishAsync(exchange: "", routingKey: "orders", true, props, body: body);
    }
}