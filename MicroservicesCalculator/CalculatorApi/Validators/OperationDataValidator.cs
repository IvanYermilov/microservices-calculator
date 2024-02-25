using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using CalculatorAPI.Models;
using FluentValidation;

namespace CalculatorAPI.Validators;

public class OperationDataValidator : AbstractValidator<OperationData>
{
    public OperationDataValidator()
    {
        RuleFor(data => data.OperationId).NotEmpty().WithMessage("Operation Id cannot be null");
        RuleFor(data => data.Expression).NotEmpty().WithMessage("Expression cannot be null or empty");
        RuleFor(data => data.Expression).MaximumLength(50)
            .WithMessage("Expression you input has length more then 50 characters");
        RuleFor(data => data.Expression).Must((expression) =>
            {
                var regExp = new Regex(Constants.ExpressionFiltrationPattern);
                return regExp.IsMatch(expression!);
            }
        ).WithMessage(
            "Expression should contain mathematical operations('+', '-', '*', '/'), numbers and symbol '.'(dot) only");
    }
}