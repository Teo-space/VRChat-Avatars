using Avatars.Services.Interfaces;
using Database.Models;

namespace Avatars.Services
{
	public class CurrentUserServise : ICurrentUserServise
	{
		public User User;
		public User Get() => User;


		public void Set(User user)
		{
			User = user;
		}
	}
}
