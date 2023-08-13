namespace Web.Areas.Manage.Models.Nomenclatures;

public class BikeTypeAdminViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; init; }
}