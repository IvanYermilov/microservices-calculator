
using CalculatorAPI.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddMassTransit(busConfigurator=>
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
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();