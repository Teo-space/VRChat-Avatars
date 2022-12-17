using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Avatars.Services
{
	public class UserClaimsService
	{

		HttpContext httpContext;
		public UserClaimsService(IHttpContextAccessor httpContextAccessor)
		{
			httpContext = httpContextAccessor.HttpContext;
		}

		public  string Claim(string claimType) => httpContext.User.FindFirstValue(claimType);

		public string Login() => Claim("login");

		public string UserId() => Claim("userid");

		public string Role() => Claim("role");

	}


}
