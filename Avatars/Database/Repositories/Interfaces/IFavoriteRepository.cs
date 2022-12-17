using Database.Models;

namespace Database.Repositories.Interfaces
{
	public interface IFavoriteRepository
	{
		public Task<List<Avatar>> GetAll(string login);
		public Task Add(string login, Avatar avatar);
		public Task<bool> Delete(string login, string avatarid);
	}
}
