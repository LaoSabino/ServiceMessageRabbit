using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class OrderContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Pedidos => Set<Order>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
}
