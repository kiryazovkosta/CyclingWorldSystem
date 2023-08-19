namespace Application.Entities.Activities.Models;

public class PagedActivityDataResponse
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalItemsCount { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }

    public List<SimplyActivityResponse> Items { get; init; } = new List<SimplyActivityResponse>();
}