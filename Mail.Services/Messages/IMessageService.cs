using Mail.Data.Domain;
using System.Collections.Generic;

namespace Mail.Services.Messages
{
	public interface IMessageService
	{
		Message GetMessageById(int id);
		Message Create(Message message);
		List<Message> GetMessages(int? fromUserId = null, int? toUserId = null);
	}
}
