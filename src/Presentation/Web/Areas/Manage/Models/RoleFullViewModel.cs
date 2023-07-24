namespace Web.Areas.Manage.Models;

public sealed record RoleFullViewModel(
    Guid Id,
    string Name,
    List<string> Users);