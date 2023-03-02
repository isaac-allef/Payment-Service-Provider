using Psp.Shared.Services.Mq;
using Psp.Worker;
using Psp.Worker.UseCases;
using Psp.Shared.Services.Db;
using Psp.Shared.Services.Db.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DbSession>(d => new DbSession(builder.Configuration["DbConnectionString"]));
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddTransient<ICustomerRepository, CustomerPostgresqlRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionPostgresqlRepository>();

builder.Services.AddTransient<ProcessTransactionHandler>();
builder.Services.AddTransient<UpdateCustomerBalanceHandler>();

builder.Services.AddScoped<RabbitMqConfiguration>(r => new RabbitMqConfiguration(builder.Configuration["RabbitMqConnectionString"]));

builder.Services.AddHostedService<TransactionsListener>();
builder.Services.AddHostedService<PayablesListener>();


var app = builder.Build();

app.MapGet("/", () => "It's up!");

app.Run();
