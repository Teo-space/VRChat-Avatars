using Database.Models;
using Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts;
using Database.Repositories.Frameworks.EntityFrameworkCore.Entities;
using Database.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Avatars.Database.Repositories
{
	internal class FavoriteRepository : IFavoriteRepository
	{
		public FavoriteRepository() { }


		IAvatarsDbContext context;
		public FavoriteRepository(IAvatarsDbContext context)
		{
			this.context = context;
		}

		public async Task<List<Avatar>> GetAll(string login)
		{
			return await context.Favorites.AsNoTracking()
				.Where(fav => fav.login == login)
				.OrderBy(fav => fav.added)
				.Include(fav => fav.Avatar)
				.Select(x => x.Avatar)
				.ProjectToType<Avatar>()
				.ToListAsync();
		}

		public async Task<List<Avatar>> GetAll1(string login)
		{
			return await context.Favorites.AsNoTracking()
				.Where(fav => fav.login == login)
				.Join(context.Avatars.AsNoTracking(), fav => fav.avatarid, avi => avi.id, (fav, avi) => new { fav, avi })
				.OrderBy(x => x.fav.added)
				.Select(x => x.avi)
				.ProjectToType<Avatar>()
				.ToListAsync();
		}
		public async Task<List<Avatar>> GetAll2(string login)
		{
			return await (from avi in context.Avatars.AsNoTracking()
						  join fav in context.Favorites.AsNoTracking() on avi.id equals fav.avatarid
						  where fav.login == login
						  orderby fav.added
						  select avi)
						 .ProjectToType<Avatar>()
						 .ToListAsync();
		}


		public async Task Add(string login, Avatar avatar)
		{
			int count = await context.Favorites.CountAsync(fav => fav.login == login);
			if (count > 1000) throw new Exception("Favorite count > limit");

			var entity = await context.Avatars.FirstOrDefaultAsync(x => x.id == avatar.id);
			if (entity == null)
			{
				await context.AddAsync(avatar.Adapt<AvatarEntity>());
			}
			else
			{
				context.Entry(entity).CurrentValues.SetValues(avatar);
			}

			await context.SaveChangesAsync();

			var fav = new FavoriteEntity()
			{
				login = login,
				avatarid = avatar.id,
				added = DateTime.Now,
			};

			context.Favorites.Add(fav);
			await context.SaveChangesAsync();
		}

		public async Task<bool> Delete(string login, string avatarid)
		{
			var entity = context.Favorites.FirstOrDefault(fav => fav.login == login && fav.avatarid == avatarid);
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
