using Database.Models;

namespace Services.Interfaces
{
	public interface IFavoriteService
	{
		public Task<List<Avatar>> GetAll(string login);
		public Task Add(string login, Avatar avatar);
		public Task<bool> Delete(string login, string avatarid);

	}
}
