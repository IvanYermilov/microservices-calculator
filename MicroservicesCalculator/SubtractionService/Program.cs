using Common.Configurations;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SubtractionService.BLL.Features.Subtraction;
using SubtractionService.BLL.SubtractionService;
using SubtractionService.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<SubtractionEventConsumer>();

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

builder.Services.AddScoped<ISubtractionOperationRepository, SubtractionOperationRepository>();

builder.Services.AddScoped<ISubtractionOperationService, SubtractionOperationService>();

var app = builder.Build();

app.Run();