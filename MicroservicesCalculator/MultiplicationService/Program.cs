using Common.Configurations;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MultiplicationService.BLL.Activities;
using MultiplicationService.DAL.Repository;
using System.Reflection;
using Common.Extensions;

#pragma warning disable CS0618
BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration.GetSection("MessageBroker:Host").Value!), h =>
        {
            h.Username(builder.Configuration.GetSection("MessageBroker:Username").Value!);
            h.Password(builder.Configuration.GetSection("MessageBroker:Password").Value!);
        });

        configurator.ConfigureEndpoints(context);
    });
    busConfigurator.AddActivitiesFromNamespaceContaining<MultiplicationActivity>();
});

builder.Services.AddMediatR((configuration) =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

var mongoDbSettings = builder.Configuration.GetSection("Database").Get<MongoDbSettings>();

builder.Services.AddMongo(mongoDbSettings);

builder.Services.AddScoped<IMultiplicationOperationRepository, MultiplicationOperationRepository>();

var app = builder.Build();

app.Run();