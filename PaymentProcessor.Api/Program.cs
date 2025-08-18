using Microsoft.OpenApi.Models;
using PaymentProcessor.Api.Infrastructure.Database;
using PaymentProcessor.Api.Infrastructure.Enum;
using PaymentProcessor.Api.Infrastructure.Redis;
using StackExchange.Redis;

var builder = WebApplication.CreateSlimBuilder(args);

var apiVersion = builder.Configuration.GetValue<string>("ApiVersion", "0.1");

var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Database connection string is not configured.");
var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Connection Strings for Redis invalid or nullable.");


builder.Services.AddSingleton<DatabaseHealthCheck>();

var redis = ConnectionMultiplexer.Connect(redisConnection);
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
//builder.Services.AddSingleton<IRabbitMQConnection>(new IRabbitMQConnection());

builder.Services.AddHttpClient(nameof(PaymentGateway.Default), httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["PaymentProcessor_Default"]!);
});

builder.Services.AddHttpClient(nameof(PaymentGateway.Fallback), httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["PaymentProcessor_Fallback"]!);
});

builder.Services.AddNpgsqlDataSource(postgresConnectionString);

ThreadPool.SetMinThreads(64, 64);

builder.Services.AddEndpointsApiExplorer();
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
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", "PaymentProcessor");
    options.DocumentTitle = "PassAuthKeeper API Documentation";
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();

await app.RunAsync();
