using Database.Models;
using System.Linq.Expressions;

namespace Database.Repositories.Interfaces
{
	public interface IAvatarRepository
	{
		public Task<Avatar> Get(string id);
		public Task<Avatar> GetWhere(Expression<Func<Avatar, bool>> predicate);
		public Task<List<Avatar>> GetAllWhere(Expression<Func<Avatar, bool>> predicate, int offset, int Limit);
		public Task<List<Avatar>> SearchByName(string name, int offset, int Limit);

		public Task Insert(Avatar avatar);
		public Task Update(Avatar avatar);
		public Task<bool> Save(Avatar avatar);
		public Task<bool> Delete(string id);

	}



}
