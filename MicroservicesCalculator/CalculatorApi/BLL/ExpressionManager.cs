using AdditionService.BLL.Activities.AdditionActivity;
using Contracts;
using MassTransit;

namespace CalculatorAPI.BLL;

public class ExpressionManager(IBus bus, IEndpointNameFormatter formatter) : IExpressionManager
{
    public async Task<decimal> Calculate(string expression, CancellationToken cancellationToken)
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
            if (decimal.TryParse(stackValue, out _))
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

    private async Task<decimal> CalculateResult(Stack<string> expressionAsStackPostfix, CancellationToken cancellationToken)
    {
        bool isFirstOperation = true;
        Stack<decimal> calculationStack = new Stack<decimal>();
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        foreach (var stackValue in expressionAsStackPostfix)
        {
            if (decimal.TryParse(stackValue, out var operand))
            {
                calculationStack.Push(operand);
            }
            else
            {
                var operand1 = calculationStack.Pop();
                var operand2 = calculationStack.Pop();
                if (isFirstOperation)
                {
                    builder.AddVariable("Operand1", operand1);

                    isFirstOperation = false;
                }

                switch (stackValue)
                {
                    case Constants.Plus:
                        calculationStack.Push(operand2 + operand1);
                        //await bus.Publish(new PlusEvent()
                        //{
                        //    Operand1 = operand1,
                        //    Operand2 = operand2
                        //}, cancellationToken);
                        builder.AddActivity("PlusActivity", new Uri($"exchange:{formatter.ExecuteActivity<AdditionActivity, OperationArguments>()}"),
                            new
                            {
                                Operand2 = operand2
                            });
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

        var routingSlip = builder.Build();

        await bus.Execute(routingSlip, cancellationToken);
        
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