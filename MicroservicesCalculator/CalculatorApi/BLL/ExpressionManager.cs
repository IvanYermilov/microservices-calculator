namespace CalculatorAPI.BLL;

public class ExpressionManager() : IExpressionManager
{
    public async Task<Stack<string>> Convert(string expression, CancellationToken cancellationToken)
    {
        Stack<string> expressionAsStackInfix = Parse(expression);
        var expressionAsStackPostfix = ToPostfixForm(expressionAsStackInfix);
        return expressionAsStackPostfix;
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