using Common.Configurations;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiplicationService.BLL.Features.Multiplication;
using MultiplicationService.BLL.MultiplicationService;
using MultiplicationService.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<MultiplicationEventConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration.GetSection("MessageBroker:Host").Value!), h =>
        {
            h.Username(builder.Configuration.GetSection("MessageBroker:Username").Value!);
            h.Password(builder.Configuration.GetSection("MessageBroker:Password").Value!);
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetSection("Database:ConnectionString").Value!));

builder.Services.AddScoped<IMultiplicationOperationRepository, MultiplicationOperationRepository>();

builder.Services.AddScoped<IMultiplicationOperationService, IMultiplicationOperationService>();

var app = builder.Build();

app.Run();