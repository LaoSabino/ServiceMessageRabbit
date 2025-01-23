using WebApplication1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.AddDbContext<OrderContext>(options => options.UseInMemoryDatabase("OrdersDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConnectionFactoryBuilder>();
builder.Services.AddHostedService<PedidoProcessorService>();

builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddHostedService<OrderConsumerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/pedido", (OrderRequest order, IOrderService orderService) =>
{
    orderService.SendOrder(order);
    return Results.Ok("Pedido enviado para processamento.");
});

app.MapPost("/pedidos", async (Order pedido, OrderContext db) =>
{
    db.Pedidos.Add(pedido);
    db.OutboxMessages.Add(new OutboxMessage
    {
        Type = "PedidoCriado",
        Content = JsonSerializer.Serialize(pedido),
        DateCreated = DateTime.UtcNow
    });
    await db.SaveChangesAsync();
    return Results.Created($"/pedidos/{pedido.Id}", pedido);
});

app.MapPut("/pedidos/{id}", async (int id, Order pedidoAtualizado, OrderContext db) =>
{
    var pedido = await db.Pedidos.FindAsync(id);
    if (pedido is null) return Results.NotFound();

    pedido.ProductName = pedidoAtualizado.ProductName;
    pedido.Quantity = pedidoAtualizado.Quantity;
    await db.SaveChangesAsync();

    db.OutboxMessages.Add(new OutboxMessage
    {
        Type = "PedidoAtualizado",
        Content = JsonSerializer.Serialize(pedido),
        DateCreated = DateTime.UtcNow
    });
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/pedidos", async (OrderContext db) =>
    await db.Pedidos.ToListAsync());

app.Run();

