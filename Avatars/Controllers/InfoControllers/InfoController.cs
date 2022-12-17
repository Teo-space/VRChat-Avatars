using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Filters;
using DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Avatars.Controllers.Info
{
	/// <summary>
	/// Api Requests Counters
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class InfoController : Controller
	{
		private static long WorkingSet() => Process.GetCurrentProcess().WorkingSet64 / 1048576;
		private static long PrivateMemory() => Process.GetCurrentProcess().PrivateMemorySize64 / 1048576;

		/// <summary>
		/// Info
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Info()
		{

			Dictionary<string, object> Result = new();

			Result.Add("WorkingSet mb : ", Process.GetCurrentProcess().WorkingSet64 / 1048576);
			Result.Add("PrivateMemory mb : ", Process.GetCurrentProcess().PrivateMemorySize64 / 1048576);

			Result.Add("Counters : ", CounterAttribute.Cashe);

			return Ok(Result);
		}
	}
}
