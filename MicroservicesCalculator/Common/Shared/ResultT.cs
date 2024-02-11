namespace Common.Shared;

public class Result<TValue> : Result
{
    public TValue Value { get; set; }
}