namespace Domain;

public class QueryParameter
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public string? OrderBy { get; init; }
    public string? FilterBy { get; init; }
    public string? SearchBy { get; init; }
}