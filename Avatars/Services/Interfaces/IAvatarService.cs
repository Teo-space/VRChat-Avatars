using Database.Models;
using System.Linq.Expressions;

namespace Services.Interfaces
{
	public interface IAvatarService
	{
		public Task<Avatar> Get(string id);



		public Task<List<Avatar>> SearchByName(string name, int offset, int Limit);
		public Task Insert(Avatar avatar);
		public Task Update(Avatar avatar);
		public Task<bool> Save(Avatar avatar);
		public Task<bool> Delete(string id);




		public Task<Avatar> GetByImage(string imageUrl);
		public Task<Avatar> GetByThumbnail(string thumbnailImageUrl);
		public Task<Avatar> GetByAsset(string assetUrl);



		public Task<List<Avatar>> GetByName(string Name, int offset, int Limit);
		public Task<List<Avatar>> GetByAuthorId(string authorId, int offset, int Limit);
		public Task<List<Avatar>> GetByAuthorName(string authorName, int offset, int Limit);
	}
}
