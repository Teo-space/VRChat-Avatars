using Avatars.Controllers.Models;
using Database.Models;
using Microsoft.AspNetCore.Http;

namespace Avatars.Database.Repositories.Interfaces
{
	public interface IAuthService
	{
		public Task<User> Authentificate(HttpRequest Request);
	}
}
