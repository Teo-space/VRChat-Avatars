using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Filters;
using AuthHandlers;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Database.Models;
using Utils.Serialization;
using Models.Input;
using System.Net;
using Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Avatars.Services.Interfaces;
using Avatars.MiddleWare.Filters;

namespace Avatars.Controllers
{
	/// <summary>
	/// Avatars
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(AuthenticationSchemes = CustomAuthSchemeOptions.Name)]
	public class AviController : Controller
	{
		//private readonly ILogger<AviController> logger;
		private readonly IAvatarService avatarService;
		//private readonly ICurrentUserServise currentUserServise;

		/// <summary>
		/// .ctor
		/// </summary>
		public AviController
		(
			//ILogger<AviController> logger, 
			IAvatarService avatarService
			//ICurrentUserServise currentUserServise
		)
		{
			//this.logger = logger;
			this.avatarService = avatarService ?? throw new ArgumentNullException(nameof(avatarService));
			//this.currentUserServise = currentUserServise;
		}

		const int Limit = 100;




		/// <summary>
		/// Get by Id
		/// </summary>
		/// <param name="avatarId"></param>
		/// <returns></returns>
		[HttpGet("{Id}")]
		//[RateLimitByUserId(1)]
		[RateLimitByIp(1)]
		//https://avatars.iteo.space/api/avi/userid
		public async Task<ActionResult<Avatar>> Get([FromRoute] AvatarId avatarId)
			=> await avatarService.Get(avatarId.Id);

		/// <summary>
		/// Create \\ Update
		/// </summary>
		/// <param name="avatar"></param>
		/// <returns></returns>
		[HttpPost]
		//[RateLimitByUserId(15)]
		[RateLimitByIp(15)]
		public async Task<ActionResult<HttpStatusCode>> Post([FromForm] Avatar avatar) 
			=> await avatarService.Save(avatar) ? HttpStatusCode.Created : HttpStatusCode.Accepted;

		/// <summary>
		/// Delete
		/// </summary>
		/// <param name="avatarId"></param>
		/// <returns></returns>
		[HttpDelete("{Id}")]
		//[RateLimitByUserId(15)]
		[RateLimitByIp(15)]
		public async Task<ActionResult<HttpStatusCode>> Delete(AvatarId avatarId) 
			=> await avatarService.Delete(avatarId.Id) ? HttpStatusCode.Accepted : HttpStatusCode.NoContent;




		/// <summary>
		/// SearchByName
		/// </summary>
		/// <param name="avatarName"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		/// https://avatars.iteo.space/api/avi/SearchByName/kanna/0
		[HttpGet("SearchByName/{Name}/{offset}")]
		//[RateLimitByUserId(4)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<List<Avatar>>> SearchByName([FromRoute] AvatarName avatarName, int offset)
			=> await avatarService.SearchByName(avatarName.Name, offset, Limit);



		/// <summary>
		/// Get By Image Url
		/// </summary>
		/// <param name="avatarUrl"></param>
		/// <returns></returns>
		[HttpGet("ByImage/{Url}")]
		//[RateLimitByUserId(3)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<Avatar>>GetByImage([FromRoute] AvatarUrl avatarUrl)
			=> await avatarService.GetByImage(avatarUrl.Url);

		/// <summary>
		/// Get By Thumbnail Image Url
		/// </summary>
		/// <param name="avatarUrl"></param>
		/// <returns></returns>
		[HttpGet("ByThumbnail/{Url}")]
		//[RateLimitByUserId(3)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<Avatar>> GetByThumbnail([FromRoute] AvatarUrl avatarUrl)
		{
			//Base64.From(allByField.FieldValue)
			return await avatarService.GetByThumbnail(avatarUrl.Url);
		}

		/// <summary>
		/// Get By Asset Url
		/// </summary>
		/// <param name="avatarUrl"></param>
		/// <returns></returns>
		[HttpGet("ByAsset/{Url}")]
		//[RateLimitByUserId(3)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<Avatar>> GetByAsset([FromRoute] AvatarUrl avatarUrl)
			=> await avatarService.GetByAsset(avatarUrl.Url);

		/// <summary>
		/// GetByName
		/// </summary>
		/// <param name="avatarName"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		[HttpGet("ByName/{Name}/{offset}")]
		//[RateLimitByUserId(4)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<List<Avatar>>> GetByName([FromRoute] AvatarName avatarName, int offset)
			=> await avatarService.GetByName(avatarName.Name, offset, Limit);

		/// <summary>
		///  GetByAuthorId
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		[HttpGet("ByAuthorId/{Id}/{offset}")]
		//[RateLimitByUserId(4)]
		[RateLimitByIp(3)]
		////https://avatars.iteo.space/api/avi/authorid/0
		public async Task<ActionResult<List<Avatar>>> GetByAuthorId([FromRoute] UserId userId, int offset)
			=> await avatarService.GetByAuthorId(userId.Id, offset, Limit);

		/// <summary>
		/// GetByAuthorName
		/// </summary>
		/// <param name="avatarName"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		[HttpGet("ByAuthorName/{Name}/{offset}")]
		//[RateLimitByUserId(4)]
		[RateLimitByIp(3)]
		public async Task<ActionResult<List<Avatar>>> GetByAuthorName([FromRoute] AvatarName avatarName, int offset)
			=> await avatarService.GetByAuthorName(avatarName.Name, offset, Limit);




	}

}
