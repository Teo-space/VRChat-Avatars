using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Net;
using System.Security.Claims;
using AuthHandlers;
using Services.Interfaces;
using Database.Models;
using Filters;
using Models.Input;
using Avatars.Services;
using Avatars.Services.Interfaces;

namespace Avatars.Controllers
{

	/// <summary>
	/// User Favorites
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = CustomAuthSchemeOptions.Name)]
	public class FavoritesController : Controller
	{

		private readonly ILogger<FavoritesController> logger;
		//private readonly UserClaimsService userClaimsService;
		private readonly ICurrentUserServise currentUserServise;
		private readonly IFavoriteService favoriteService;

		private readonly User CurrentUser;

		/// <summary>
		/// .ctor
		/// </summary>
		public FavoritesController
		(
			ILogger<FavoritesController> logger,
			//UserClaimsService userClaimsService,
			ICurrentUserServise currentUserServise,
			IFavoriteService favoriteService
		)
		{
			this.logger = logger;
			//this.userClaimsService = userClaimsService;
			this.currentUserServise = currentUserServise;
			this.CurrentUser = currentUserServise.Get();
			this.favoriteService = favoriteService;
		}

		/// <summary>
		/// Get All User Favorites
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[RateLimitByUserId(5)]
		public async Task<ActionResult<List<Avatar>>> Get()
		{
			//return await favoriteService.GetAll(this.Login());
			return await favoriteService.GetAll(CurrentUser.login);
		}

		/// <summary>
		/// Add User Favorite
		/// </summary>
		/// <param name="avatar"></param>
		/// <returns></returns>
		[HttpPost]
		[RateLimitByUserId(1)]
		public async Task<IActionResult> Add([FromForm] Avatar avatar)
		{
			await favoriteService.Add(CurrentUser.login, avatar);
			return StatusCode((int)HttpStatusCode.Created);
		}

		/// <summary>
		/// Delete Fav
		/// </summary>
		/// <param name="avatarId"></param>
		/// <returns></returns>
		[HttpDelete("{Id}")]
		[RateLimitByUserId(1)]
		public async Task<IActionResult> Delete([FromRoute] AvatarId avatarId)
		{
			//if (await favoriteService.Delete(this.Login(), avatarId.Id))
			if (await favoriteService.Delete(CurrentUser.login, avatarId.Id))
			{
				return StatusCode((int)HttpStatusCode.OK);
			}
			else return StatusCode((int)HttpStatusCode.NoContent);
		}





	}
}
