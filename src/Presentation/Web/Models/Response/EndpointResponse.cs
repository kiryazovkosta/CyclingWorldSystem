namespace Web.Models.Response;

public class EndpointResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public T? Value { get; set; }
    public bool IsFailure => !IsSuccess;
}
