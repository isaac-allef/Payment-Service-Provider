using Psp.Api.Caching;
using Psp.Api.Db;
using Psp.Shared.Services.Db;
using Psp.Shared.Services.Mq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());;

builder.Services.AddScoped<RabbitMqConfiguration>(
    r => new RabbitMqConfiguration(builder.Configuration["RabbitMqConnectionString"]));

builder.Services.AddScoped<DbSession>(
    x => new DbSession(builder.Configuration["DbConnectionString"]));

builder.Services.AddScoped<TransactionFacade>();
builder.Services.AddScoped<BalanceFacade>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisConnectionString"];
    options.InstanceName = "instance";
});
builder.Services.AddScoped<ICaching, RedisAdapter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => "It's up!");

app.Run();
