using Database.Models;
using Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts;
using Database.Repositories.Frameworks.EntityFrameworkCore.Entities;
using Database.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Avatars.Database.Repositories
{
	public class AvatarRepository : IAvatarRepository
	{
		public AvatarRepository() { }
		IAvatarsDbContext context;
		public AvatarRepository(IAvatarsDbContext context)
		{
			this.context = context;
		}
		

		public async Task<Avatar> Get(string id)
		{
			//return await context.Avatars.AsNoTracking().Where(avi => avi.id == id).ProjectToType<Avatar>().FirstOrDefaultAsync();
			return (await context.Avatars.AsNoTracking().FirstOrDefaultAsync(avi => avi.id == id)).Adapt<Avatar>();
		}

		public async Task<Avatar> GetWhere(Expression<Func<Avatar, bool>> predicate)
		{
			return (await context.Avatars.AsNoTracking().FirstOrDefaultAsync(predicate)).Adapt<Avatar>();
		}

		public async Task<List<Avatar>> GetAllWhere(Expression<Func<Avatar, bool>> predicate, int offset, int Limit)
		{
			return await context.Avatars
				.AsNoTracking()
				.Where(predicate)
				.Skip(offset)
				.Take(Limit)
				.ProjectToType<Avatar>()
				.ToListAsync();
		}

		//SELECT * FROM Avatars WHERE MATCH(Avatars.name) AGAINST ('Kanna') Limit 1000, 100;
		public async Task<List<Avatar>> SearchByName(string name, int offset, int Limit)
		{
			return await context.Avatars
				.AsNoTracking()
				.Where(x => EF.Functions.Match(new[] { x.name }, name, MySqlMatchSearchMode.NaturalLanguage))
				.Skip(offset)
				.Take(Limit)
				.ProjectToType<Avatar>()
				.ToListAsync();
		}



		public async Task Insert(Avatar avatar)
		{
			AvatarEntity avatarEntity = avatar.Adapt<AvatarEntity>();

			await context.Avatars.AddAsync(avatarEntity);
			await context.SaveChangesAsync();
		}

		public async Task Update(Avatar avatar)
		{
			var entity = await context.Avatars.FirstOrDefaultAsync(x => x.id == avatar.id);
			if (entity != null)
			{
				context.Entry(entity).CurrentValues.SetValues(avatar);
				await context.SaveChangesAsync();
			}
		}

		public async Task<bool> Save(Avatar avatar)
		{
			var entity = await context.Avatars.FirstOrDefaultAsync(x => x.id == avatar.id);
			if (entity == null)
			{
				await context.AddAsync(avatar.Adapt<AvatarEntity>());
				await context.SaveChangesAsync();
				return true;
			}
			else
			{
				context.Entry(entity).CurrentValues.SetValues(avatar);
				await context.SaveChangesAsync();
				return false;
			}
		}

		public async Task<bool> Delete(string id)
		{
			var entity = context.Avatars.Include(avi => avi.Favorites).FirstOrDefault(avi => avi.id == id);
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
