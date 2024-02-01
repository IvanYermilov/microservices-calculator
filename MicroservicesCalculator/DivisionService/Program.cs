using Common.Configurations;
using DivisionService.BLL.DivisionService;
using DivisionService.BLL.Features.Division;
using DivisionService.DAL.Repository;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<DivisionEventConsumer>();

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

builder.Services.AddScoped<IDivisionOperationRepository, DivisionOperationRepository>();

builder.Services.AddScoped<IDivisionService, DivisionOperationService>();

var app = builder.Build();

app.Run();