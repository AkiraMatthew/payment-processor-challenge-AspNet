using Microsoft.OpenApi.Models;
using PaymentProcessor.Api.Infrastructure.Database;
using StackExchange.Redis;

var builder = WebApplication.CreateSlimBuilder(args);

var apiVersion = builder.Configuration.GetValue<string>("ApiVersion");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<DatabaseHealthCheck>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

builder.Services.AddHttpClient();

#region Swagger Documentation
builder.Services.AddSwaggerGen(options =>
{
    /*options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
                    "This is a system built for encrypt/decrypt passwords and files, generate passwords, store passwords/sensitive data\r\n\r\n" +
                    "Type 'Bearer' + your token in the input below, ex.: 'Bearer XYZ'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });*/
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
#endregion

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
    options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", "ADS.PassAuthKeeper");
    options.DocumentTitle = "PassAuthKeeper API Documentation";
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();

//app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
