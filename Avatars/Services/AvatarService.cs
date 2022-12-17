using Database.Models;
using Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts;
using Database.Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
	public class AvatarService : IAvatarService
	{
		public AvatarService() { }
		IAvatarRepository avatarRepository;
		public AvatarService(IAvatarRepository avatarRepository)
		{
			this.avatarRepository = avatarRepository;
		}

		public Task<Avatar> Get(string id)
			=> avatarRepository.Get(id);



		public Task<List<Avatar>> SearchByName(string name, int offset, int Limit)
			=> avatarRepository.SearchByName(name, offset, Limit);

		public Task Insert(Avatar avatar)
			=> avatarRepository.Insert(avatar);
		public Task Update(Avatar avatar)
			=> avatarRepository.Update(avatar);
		public Task<bool> Save(Avatar avatar)
			=> avatarRepository.Save(avatar);
		public Task<bool> Delete(string id)
			=> avatarRepository.Delete(id);




		public Task<Avatar> GetByImage(string imageUrl)
			=> avatarRepository.GetWhere(x => x.imageUrl == imageUrl);
		public Task<Avatar> GetByThumbnail(string thumbnailImageUrl)
			=> avatarRepository.GetWhere(x => x.thumbnailImageUrl == thumbnailImageUrl);
		public Task<Avatar> GetByAsset(string assetUrl)
			=> avatarRepository.GetWhere(x => x.assetUrl == assetUrl);



		public Task<List<Avatar>> GetByName(string Name, int offset, int Limit)
			=> avatarRepository.GetAllWhere(x => x.name == Name, offset, Limit);
		public Task<List<Avatar>> GetByAuthorId(string authorId, int offset, int Limit)
			=> avatarRepository.GetAllWhere(x => x.authorId == authorId, offset, Limit);
		public Task<List<Avatar>> GetByAuthorName(string authorName, int offset, int Limit)
			=> avatarRepository.GetAllWhere(x => x.authorName == authorName, offset, Limit);




	}
}
