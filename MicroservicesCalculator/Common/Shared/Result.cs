namespace Common.Shared;

public class Result
{
    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public IList<string> Errors = new List<string>();
}