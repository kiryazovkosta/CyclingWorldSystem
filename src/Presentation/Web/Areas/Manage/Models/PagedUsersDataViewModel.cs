namespace Web.Areas.Manage.Models;

public class PagedUsersDataViewModel
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalItemsCount { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }
    
    public string? OrderBy { get; init; }
    public string? FilterBy { get; init; }
    public string? SearchBy { get; init; }
    public List<UserViewModel> Items { get; init; } = new List<UserViewModel>();
}