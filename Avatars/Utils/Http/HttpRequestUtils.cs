using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Avatars.Utils.Http
{
	internal static class HttpRequestUtils
	{
		//UnityPlayer/2018.4.20f1 (UnityWebRequest/1.0, libcurl/7.52.0-DEV)
		public static bool WrongUserAgent(this HttpRequest Request)
		{
			string UserAgent = Request.UserAgent();
			if (UserAgent != null && UserAgent.StartsWith("UnityPlayer") && UserAgent.Contains("UnityWebRequest")) return false;
			return true;
		}



		public static string Header(this HttpRequest Request, string key)
		{
			try
			{
				if (Request != null && Request.Headers != null)
				{
					Microsoft.Extensions.Primitives.StringValues result;

					if (Request.Headers.TryGetValue(key, out result) || Request.Headers.TryGetValue(key.ToLower(), out result))
					{
						return result;
					}
				}
			}
			catch { }
			return string.Empty;
		}


		public static string UserAgent(this HttpRequest Request) => Header(Request, "User-Agent");
		public static string Forwarded(this HttpRequest Request) => Header(Request, "x-forwarded-for");






	}

}
