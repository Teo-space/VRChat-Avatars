using Database.Models;
using System.Linq.Expressions;

namespace Database.Repositories.Interfaces
{
	public interface IFindRepo<T>
	{
		public Task<T> Find(object id);
	}



	//IQueryable

	public interface IUserRepository
	{
		public IQueryable<User> AsNotracking();


		public Task<User> Get(string login);
		public Task<User> GetByToken(string token);

		public Task<User> Get(Expression<Func<User, bool>> predicate);
		public Task<List<User>> GetAll(Expression<Func<User, bool>> predicate);
		public Task<List<TResult>> GetAll<TResult>(
			Expression<Func<User, bool>> Where,
			Expression<Func<User, TResult>> selector);
		public Task<long> Count(Expression<Func<User, bool>> predicate);



		public Task Insert(User user);
		public Task Update(User user);
		public Task<bool> Save(User user);
		public Task<bool> Delete(string login);
	}
}
