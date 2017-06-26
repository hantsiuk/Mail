using System;

namespace Mail.Data.Domain
{
	public class Message
	{
		public int Id { get; set; }

		public string Subject { get; set; }
	
		public string Body { get; set; }

		public int FromUserId { get; set; }

		public int ToUserId { get; set; }

		public DateTime CreationTime { get; set; }
	}
}
