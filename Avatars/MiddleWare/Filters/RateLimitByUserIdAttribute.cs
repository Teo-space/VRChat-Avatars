using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Claims;


namespace Filters
{

	internal class RateLimitByUserIdAttribute : ActionFilterAttribute
	{
		private readonly int Seconds = 0;

		static readonly ConcurrentDictionary<string, DateTime> Cashe = new();


		public RateLimitByUserIdAttribute(int Seconds = 2)
		{
			this.Seconds = Seconds;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			try
			{
				string userid = context.HttpContext.User.FindFirstValue("userid");
				if (string.IsNullOrEmpty(userid))
				{
					context.Result = new ObjectResult("Request RateLimited(claim userid not found)") { StatusCode = 429 };
				}

				string id = $"{context.RouteData.Values["Controller"]}.{context.HttpContext.Request.Method}.{context.RouteData.Values["action"]}.{userid}";

				if (Cashe.TryGetValue(id, out DateTime dateTime) && dateTime.AddSeconds(Seconds) > DateTime.Now)
				{
					context.Result = new ObjectResult("Request RateLimited(RateLimit)") { StatusCode = 429 };
				}
				else
				{
					Cashe[id] = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				context.Result = new ObjectResult($"Exception on RateLimitAttribute: {ex.Message}") { StatusCode = 500 };
			}

			base.OnActionExecuting(context);
		}
	}


}
