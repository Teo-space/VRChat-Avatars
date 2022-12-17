using Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Avatars.Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts.Interfaces;
using Avatars.Database.Models;

namespace DbContexts
{

	public interface IUserDbContext : IDbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<AuthLog> AuthLogs { get; set; }
	}


	public class UserDbContext : DbContext, IUserDbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<AuthLog> AuthLogs{ get; set; }

		public UserDbContext() { }

		IConfiguration configuration;
		public UserDbContext(IConfiguration configuration)
		{
			this.configuration = configuration;
			//Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseMySql(ConnectionStrings.UsersRemote, ServerVersion.AutoDetect(ConnectionStrings.UsersRemote));

			string cs = this.configuration.GetConnectionString("UsersRemote");
			optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));
		}

	}
}
