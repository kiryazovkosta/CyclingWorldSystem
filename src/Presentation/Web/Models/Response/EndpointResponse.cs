namespace Web.Models.Response;

public class EndpointResponse<T>
{
    public bool IsSuccess { get; set; }
    public RestError? Error { get; set; }
    public T? Value { get; set; }
    public bool IsFailure => !IsSuccess;
}

public class RestError
{
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class ValidateEntityError
{
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public Dictionary<string, string[]> errors { get; set; }
}

public class ValidationError
{
    public string[] Name { get; set; }
}
