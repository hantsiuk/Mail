using Mail.Data;

namespace Mail.Services.Messages
{
	public class MessageService
	{
		private readonly IDbContext dbContext;

		public MessageService(IDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
	}
}
