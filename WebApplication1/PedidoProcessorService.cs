namespace WebApplication1;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class PedidoProcessorService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderContext>();

            var mensagensOutbox = await db.OutboxMessages
                .Where(m => m.DateCreated <= DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            foreach (var mensagem in mensagensOutbox)
            {
                // Processar mensagem (ex: enviar para RabbitMQ)
                Console.WriteLine($"Processando mensagem: {mensagem.Type}, Conteúdo: {mensagem.Content}");

                db.OutboxMessages.Remove(mensagem);
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(_intervalo, stoppingToken);
        }
    }
}
