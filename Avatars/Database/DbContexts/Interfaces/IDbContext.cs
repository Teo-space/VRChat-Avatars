using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace Avatars.Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts.Interfaces
{
	public interface IDbContext
	{

		public EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
		public EntityEntry<TEntity> Add<TEntity>([NotNull] TEntity entity) where TEntity : class;
		public ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([NotNull] TEntity entity,
			CancellationToken cancellationToken = default) where TEntity : class;

		public EntityEntry<TEntity> Attach<TEntity>([NotNull] TEntity entity) where TEntity : class;
		public EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class;
		//public EntityEntry<TEntity> Save<TEntity>([NotNull] TEntity entity) where TEntity : class;
		public EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity) where TEntity : class;

		


		public int SaveChanges();
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
	}
}
