namespace Web.Models.Authorization
{
	public sealed class LogInUserInputModel
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; }
	}
}
