using Saga.Orchestrator.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("OrderService", c => c.BaseAddress =new Uri("http://localhost:5120/"));
builder.Services.AddHttpClient("InventoryService", c => c.BaseAddress =new Uri("http://localhost:5130/"));
builder.Services.AddHttpClient("NotificationService", c => c.BaseAddress =new Uri("http://localhost:5140/"));

builder.Services.AddSingleton<IOrderProxy,OrderProxy>();
builder.Services.AddSingleton<IInventoryProxy,InventoryProxy>();
builder.Services.AddSingleton<INotificationProxy,NotificationProxy>();
builder.Services.AddSingleton<IOrderManager,OrderManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
