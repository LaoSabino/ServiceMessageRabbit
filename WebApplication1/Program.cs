using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConnectionFactoryBuilder>();
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

app.Run();

