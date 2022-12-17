using Database.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Database.Repositories.Frameworks.EntityFrameworkCore.Entities
{
	public class FavoriteEntity : Favorite
	{

		[JsonIgnore]
		public AvatarEntity Avatar { get; set; }
	}


}
