using Mail.Data;

namespace Mail.Services.Users
{
	public class UserService
	{
		private readonly IDbContext dbContext;

		public UserService(IDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
	}
}
