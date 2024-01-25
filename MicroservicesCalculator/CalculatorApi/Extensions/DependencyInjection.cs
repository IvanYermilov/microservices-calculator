using CalculatorAPI.BLL;
using CalculatorAPI.Validators;
using FluentValidation;

namespace CalculatorAPI.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OperationDataValidator>();
        services.AddScoped<IExpressionManager, ExpressionManager>();

        return services;
    }
}