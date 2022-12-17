using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Avatars.Controllers
{
	public static class ClaimExtentions
	{
		public static string Claim(this ControllerBase controllerBase, string claimType)
		{
			//return controllerBase.User?.FindFirstValue(claimType) ?? "";
			return controllerBase.User.FindFirstValue(claimType);
		}

		public static string Login(this ControllerBase controllerBase)
			=> controllerBase.Claim("login");

		public static string UserId(this ControllerBase controllerBase)
			=> controllerBase.Claim("userid");

		public static string Role(this ControllerBase controllerBase)
			=> controllerBase.Claim("role");
	}
}
