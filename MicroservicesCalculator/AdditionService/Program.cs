using System.Reflection;
using AdditionService.BLL.Activities.AdditionActivity;
using AdditionService.DAL.Repository;
using Common.Configurations;
using Common.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

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
    busConfigurator.AddActivitiesFromNamespaceContaining<AdditionActivity>();
});

builder.Services.AddMediatR((configuration) =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton(sp =>                  //Проверить, нужны ли Options, если написал то, что на 46
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

 var mongoDbSettings = builder.Configuration.GetSection("Database").Get<MongoDbSettings>();

builder.Services.AddMongo(mongoDbSettings);

builder.Services.AddScoped<IAdditionOperationRepository, AdditionOperationRepository>();

var app = builder.Build();

app.Run();
