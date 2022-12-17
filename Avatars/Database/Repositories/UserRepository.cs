using Database.Models;
using Database.Repositories.Interfaces;
using DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Avatars.Database.Repositories
{





	public class UserRepository : IUserRepository
	{
		public UserRepository() { }
		IUserDbContext context;
		public UserRepository(IUserDbContext context)
		{
			this.context = context;
		}

		public IQueryable<User> AsNotracking() => context.Users.AsNoTracking();


		public async Task<User> Get(string login)
		{
			return await context.Users.AsNoTracking().FirstOrDefaultAsync(usr => usr.login == login);
		}

		public async Task<User> GetByToken(string token)
		{
			return await context.Users.AsNoTracking().FirstOrDefaultAsync(usr => usr.authToken == token);
		}

		public async Task<User> Get(Expression<Func<User, bool>> predicate)
		{
			return await context.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
		}

		public async Task<List<User>>GetAll(Expression<Func<User, bool>> Where)
		{
			return await context.Users.AsNoTracking().Where(Where).ToListAsync();
		}

		public async Task<List<TResult>> GetAll<TResult>(
			Expression<Func<User, bool>> Where,
			Expression<Func<User, TResult>> selector)
		{
			return await context.Users
				.AsNoTracking()
				.Where(Where)
				.Select(selector)
				.ToListAsync();
		}

		public async Task<long> Count(Expression<Func<User, bool>> predicate)
		{
			return await context.Users.CountAsync(predicate);
		}





		public async Task Insert(User user)
		{
			await context.AddAsync(user);
			await context.SaveChangesAsync();
		}

		public async Task Update(User user)
		{
			var entity = await context.Users.FirstOrDefaultAsync(usr => usr.login == user.login);
			if (entity != null)
			{
				context.Entry(entity).CurrentValues.SetValues(user);
				await context.SaveChangesAsync();
			}
		}

		public async Task<bool> Save(User user)
		{
			var entity = await context.Users.FirstOrDefaultAsync(usr => usr.login == user.login);
			if(entity == null)
			{
				await context.AddAsync(user);
				await context.SaveChangesAsync();
				return true;
			}
			else
			{
				context.Entry(entity).CurrentValues.SetValues(user);
				await context.SaveChangesAsync();
				return false;
			}
		}

		public async Task<bool> Delete(string login)
		{
			var entity = context.Users.FirstOrDefault(usr => usr.login == login);
			if (entity != null)
			{
				context.Remove(entity);
				await context.SaveChangesAsync();
				return true;
			}
			return false;
		}



	}

}
