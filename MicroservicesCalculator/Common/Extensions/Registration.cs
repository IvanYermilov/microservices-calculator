using Common.Configurations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common.Extensions;

public static class Registration
{
    public static IServiceCollection AddMongo(this IServiceCollection services, MongoDbSettings mongoDbSettings)
    {
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(mongoDbSettings.ConnectionString));

        return services;
    }
}