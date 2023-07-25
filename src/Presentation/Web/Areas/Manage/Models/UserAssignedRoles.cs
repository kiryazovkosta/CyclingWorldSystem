namespace Web.Areas.Manage.Models;
public class UserAssignedRoles
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public List<string> MemberOfRoles { get; set; } = new List<string>();
    public List<string> NotMemberOfRoles { get; set; } = new List<string>();
}