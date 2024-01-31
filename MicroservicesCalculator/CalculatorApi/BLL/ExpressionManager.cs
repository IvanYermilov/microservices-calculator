
using Contracts;
using MassTransit;

namespace CalculatorAPI.BLL;

public class ExpressionManager(IPublishEndpoint publishEndpoint) : IExpressionManager
{
    public async Task<double> Calculate(string expression, CancellationToken cancellationToken)
    {
        Stack<string> expressionAsStackInfix = Parse(expression);
        var expressionAsStackPostfix = ToPostfixForm(expressionAsStackInfix);
        return await CalculateResult(expressionAsStackPostfix, cancellationToken); 
    }


    private Stack<string> Parse(string expression)
    {
        Stack<string> stack = new Stack<string>();


        bool isPreviousStackValueFilled = true;
        foreach (var character in expression)
        {
            string ch = character.ToString();
            if (ch.Equals(Constants.Minus))
            {
                if (stack.TryPeek(out string? previousStackValue))
                {
                    if (Constants.AllowedOperands.Contains(previousStackValue))
                    {
                        isPreviousStackValueFilled = false;
                        stack.Push(ch);
                    }
                    else
                    {
                        isPreviousStackValueFilled = true;
                        stack.Push(ch);
                    }
                }
                else
                {
                    isPreviousStackValueFilled = false;
                    stack.Push(ch);
                }
            }
            else
            {
                if (Constants.AllowedOperands.Contains(ch))
                {
                    stack.Push(ch);
                    isPreviousStackValueFilled = true;
                    continue;
                }
                if (!isPreviousStackValueFilled)
                {
                    string intermediateValue = string.Concat(stack.Pop(), ch);
                    stack.Push(intermediateValue);
                }
                else
                {
                    isPreviousStackValueFilled = false;
                    stack.Push(ch);
                }
            }
        }

        return ReverseStack(stack);
    }

    private static Stack<string> ToPostfixForm(Stack<string> expression)
    {
        Stack<string> tempStack = new Stack<string>();
        Stack<string> finalStack = new Stack<string>();

        foreach (var stackValue in expression)
        {
            if (double.TryParse(stackValue, out _))
            {
                finalStack.Push(stackValue);
            }
            else
            {
                AllocateStackValue(stackValue, tempStack, finalStack);
            }
        }

        if (tempStack.Count != 0)
        {
            do
            {
                var tempStackTopValue = tempStack.Pop();
                finalStack.Push(tempStackTopValue);
            } while (tempStack.Count != 0);
        }

        return ReverseStack(finalStack);
    }

    private static void AllocateStackValue(string? value, Stack<string> tempStack, Stack<string> finalStack)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            var tempStackTopValue = tempStack.Pop();
            AllocateStackValue(tempStackTopValue, tempStack, finalStack);
        }
        else if (tempStack.Count == 0)
        {
            tempStack.Push(value);
        }
        else
        {
            if (value.Equals(Constants.Divide) || value.Equals(Constants.Multiply))
            {
                if (!tempStack.TryPeek(out var tempStackTopValueCopy))
                {
                    tempStack.Push(value);
                    return;
                }
                if (!tempStackTopValueCopy.Equals(Constants.Multiply) &&
                    !tempStackTopValueCopy.Equals(Constants.Divide))
                {
                    tempStack.Push(value);
                    return;
                }

            }

            var tempStackTopValue = tempStack.Pop();
            finalStack.Push(tempStackTopValue);
            AllocateStackValue(value, tempStack, finalStack);
        }
    }

    private async Task<double> CalculateResult(Stack<string> expressionAsStackPostfix, CancellationToken cancellationToken)
    {
        Stack<double> calculationStack = new Stack<double>();
        foreach (var stackValue in expressionAsStackPostfix)
        {
            if (double.TryParse(stackValue, out var operand))
            {
                calculationStack.Push(operand);
            }
            else
            {
                var operand1 = calculationStack.Pop();
                var operand2 = calculationStack.Pop();

                switch (stackValue)
                {
                    case Constants.Plus:
                        calculationStack.Push(operand2 + operand1);
                        await publishEndpoint.Publish(new PlusEvent()
                        {
                            Operand1 = operand1,
                            Operand2 = operand2
                        }, cancellationToken);
                        break;
                    case Constants.Minus:
                        calculationStack.Push(operand2 - operand1);
                        break;
                    case Constants.Multiply:
                        calculationStack.Push(operand2 * operand1);
                        break;
                    case Constants.Divide:
                        calculationStack.Push(operand2 / operand1);
                        break;
                }
            }
        }
        return calculationStack.Pop();
    }

    static Stack<T> ReverseStack<T>(Stack<T> stack)
    {
        Stack<T> reversedStack = new Stack<T>();

        while (stack.Count > 0)
        {
            reversedStack.Push(stack.Pop());
        }

        return reversedStack;
    }
}