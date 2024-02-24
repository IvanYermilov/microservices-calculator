using System.Reflection;
using CalculatorAPI.Extensions;
using CalculatorAPI.Messaging.Consumers;
using CalculatorAPI.Messaging.StateMachines;
using Common.Configurations;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration.GetSection("MessageBroker:Host").Value!), h =>
        {
            h.Username(builder.Configuration.GetSection("MessageBroker:Username").Value!);
            h.Password(builder.Configuration.GetSection("MessageBroker:Password").Value!);
        });

        configurator.UseInMemoryOutbox(context);

        configurator.ConfigureEndpoints(context);
    });

    busConfigurator.AddSagaStateMachine<CalculateExpressionStateMachine, CalculateExpressionState>()
        .MongoDbRepository(r =>
        {
            r.Connection = builder.Configuration.GetSection("Database:ConnectionString").Value!;
            r.DatabaseName = builder.Configuration.GetSection("Database:DatabaseName").Value!;
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();