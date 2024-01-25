using System.Text.RegularExpressions;

namespace CalculatorAPI;

public static class Constants
{
    public static readonly string ExpressionFiltrationPattern = @"^((-{0,1})\d+|(-{0,1})(\d+\.\d+))([*+/-]{1}(((-{0,1})\d+)|((-{0,1})(\d+\.\d+))))+$";

    public const string Minus = "-";
    public const string Plus = "+";
    public const string Multiply = "*";
    public const string Divide = "/";

    public static readonly string[] AllowedOperands =
    [
        Minus,
        Plus,
        Multiply,
        Divide
    ];
}