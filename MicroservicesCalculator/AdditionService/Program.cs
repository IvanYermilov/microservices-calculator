using AdditionService.BLL.AdditionService;
using AdditionService.BLL.Features.Addition;
using AdditionService.Configurations;
using AdditionService.DAL.Repository;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<AdditionEventConsumer>();

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

builder.Services.AddScoped<IAdditionOperationRepository, AdditionOperationRepository>();

builder.Services.AddScoped<IAdditionService, AdditionOperationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run();

