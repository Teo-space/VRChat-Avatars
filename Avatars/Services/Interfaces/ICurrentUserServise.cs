using Database.Models;

namespace Avatars.Services.Interfaces
{
	public interface ICurrentUserServise
	{
		public User Get();
		public void Set(User user);
	}

}
