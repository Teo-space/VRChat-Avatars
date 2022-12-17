using Avatars.Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts.Interfaces;
using Database.Repositories.Frameworks.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts
{

	public interface IAvatarsDbContext : IDbContext
	{
		public DbSet<AvatarEntity> Avatars { get; set; }
		public DbSet<FavoriteEntity> Favorites { get; set; }
	}




	public class AvatarsDbContext : DbContext, IAvatarsDbContext
	{

		public DbSet<AvatarEntity> Avatars { get; set; }
		public DbSet<FavoriteEntity> Favorites { get; set; }

		public AvatarsDbContext() { }


		IConfiguration configuration;
		public AvatarsDbContext(IConfiguration configuration)
		{
			this.configuration = configuration;
		}



		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseMySql(ConnectionStrings.AvatarsRemote, ServerVersion.AutoDetect(ConnectionStrings.AvatarsRemote));
			string cs = this.configuration.GetConnectionString("AvatarsRemote");
			optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AvatarEntity>()
				.HasKey(x => new { x.id });

			modelBuilder.Entity<FavoriteEntity>()
				.HasKey(x => new { x.login, x.avatarid });
			modelBuilder.Entity<FavoriteEntity>()
				.HasOne(fav => fav.Avatar)
				.WithMany(avi => avi.Favorites)
				.HasForeignKey(fav => fav.avatarid);
		}
	}
}
