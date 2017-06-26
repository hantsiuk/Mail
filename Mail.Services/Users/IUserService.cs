using Mail.Data.Domain;
using System.Collections.Generic;

namespace Mail.Services.Users
{
	public interface IUserService
	{
		User GetUserById(int Id);
		User GetUserByUserName(string userName);
		User CreateUser(User user);
		List<User> GetAll();
	}
}
