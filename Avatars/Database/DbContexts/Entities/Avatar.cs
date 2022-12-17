//using AutoMapper;
using System.Text.Json.Serialization;
using Database.Models;

namespace Database.Repositories.Frameworks.EntityFrameworkCore.Entities
{

	public class AvatarEntity : Avatar
	{

		[JsonIgnore]
		public List<FavoriteEntity> Favorites { get; set; }
	}
}
