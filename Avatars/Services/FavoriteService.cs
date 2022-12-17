using Database.Models;
using Database.Repositories;
using Database.Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
	public class FavoriteService : IFavoriteService
	{
		private readonly IFavoriteRepository favoriteRepository;
		public FavoriteService(IFavoriteRepository favoriteRepository)
		{
			this.favoriteRepository = favoriteRepository;
		}

		public Task<List<Avatar>> GetAll(string login) => this.favoriteRepository.GetAll(login);
		public Task Add(string login, Avatar avatar) => this.favoriteRepository.Add(login, avatar);
		public Task<bool> Delete(string login, string avatarid) => this.favoriteRepository.Delete(login, avatarid);

	}
}
