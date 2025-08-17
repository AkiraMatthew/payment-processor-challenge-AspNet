using Microsoft.OpenApi.Models;
using PaymentProcessor.Api.Infrastructure.Database;
using PaymentProcessor.Api.Infrastructure.Enum;
using PaymentProcessor.Api.Infrastructure.MessageBroker;
using PaymentProcessor.Api.Infrastructure.Redis;

var builder = WebApplication.CreateSlimBuilder(args);

var apiVersion = builder.Configuration.GetValue<string>("ApiVersion");
var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Database connection string is not configured.");

builder.Services.AddSingleton<DatabaseHealthCheck>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
//builder.Services.AddSingleton<IRabbitMQConnection>(new IRabbitMQConnection());

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("Redis");
    opts.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
    {
        AbortOnConnectFail = true,
        EndPoints = { opts.Configuration! }
    };
});

builder.Services.AddHttpClient(nameof(PaymentGateway.Default), httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["PaymentProcessor_Default"]!);
});

builder.Services.AddHttpClient(nameof(PaymentGateway.Default), httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["PaymentProcessor_Fallback"]!);
});

builder.Services.AddNpgsqlDataSource(postgresConnectionString);

ThreadPool.SetMinThreads(32, 32);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Title = "Payment Processor",
        Version = apiVersion,
        Description = "This payment processor works as a proxy that will proccess Brazil's Central Bank financial transactions",
        Contact = new OpenApiContact
        {
            Name = "Mateus Henrique",
            Email = "akiradigitalsolutionss@gmail.com",
            Url = new Uri("https://akiradigitalsolutions.com")
        }
    });
});

var app = builder.Build();

#region Database HealthCheck
var healthCheck = app.Services.GetRequiredService<DatabaseHealthCheck>();
if (!await healthCheck.IsDatabaseReady())
{
    throw new Exception("PostgreSQL connection failed!");
}
#endregion

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", "PaymentProcessor");
    options.DocumentTitle = "PassAuthKeeper API Documentation";
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();

await app.RunAsync();
