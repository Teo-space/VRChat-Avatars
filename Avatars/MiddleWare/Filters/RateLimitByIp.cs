using Avatars.Controllers.Responses;
using Avatars.Utils.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace Avatars.MiddleWare.Filters
{
	public class RateLimitByIp : ActionFilterAttribute
	{
		private readonly int Seconds = 0;

		static readonly ConcurrentDictionary<string, DateTime> Cashe = new();

		public RateLimitByIp(int Seconds = 2)
		{
			this.Seconds = Seconds;
		}


		public override void OnActionExecuting(ActionExecutingContext context)
		{
			try
			{
				string Forwarded = HttpRequestUtils.Forwarded(context.HttpContext.Request);
				string RemoteIpAddress = HttpResponseUtils.RemoteIpAddress(context.HttpContext.Response);
				string ip = string.IsNullOrEmpty(Forwarded) ? Forwarded : RemoteIpAddress;

				string id = $"{context.RouteData.Values["Controller"]}.{context.HttpContext.Request.Method}.{context.RouteData.Values["action"]}.{ip}";

				if (Cashe.TryGetValue(id, out DateTime dateTime) && dateTime.AddSeconds(Seconds) > DateTime.Now)
				{
					context.Result = JsonMessageCrt.Result(false, $"Request RateLimited({(dateTime.AddSeconds(Seconds) - DateTime.Now).TotalMilliseconds})");




					/*
					context.Result = new JsonResult
					(
						


						JsonMessageCrt.Result(false, $"Request RateLimited({(dateTime.AddSeconds(Seconds) - DateTime.Now).TotalMilliseconds})")
					) 
					{
						StatusCode = 429
					};
					*/
				}
				else
				{
					Cashe[id] = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				context.Result = JsonMessageCrt.Result(false, $"Exception on RateLimitAttribute: {ex.Message}");

				/*
				context.Result = new ObjectResult
				(
					JsonMessageCrt.Result(false, $"Exception on RateLimitAttribute: {ex.Message}")
				)
				{ 
					StatusCode = 500 
				};
				*/
			}

			base.OnActionExecuting(context);
		}

	}




}
