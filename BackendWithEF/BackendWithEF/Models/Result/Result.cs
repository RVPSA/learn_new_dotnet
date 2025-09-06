namespace BackendWithEF.Models.Result;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string Error { get; set; }

    protected Result(bool isSuccess, T? value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty);
    public static Result<T> Fail(string error) => new Result<T>(false,default, error);
}