using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avatars.Utils.Http
{
	public static class HttpResponseUtils
	{
		public static string RemoteIpAddress(HttpResponse Response)
		{
			if (Response == null) return null;
			if (Response.HttpContext == null) return null;
			if (Response.HttpContext.Connection == null) return null;
			if (Response.HttpContext.Connection.RemoteIpAddress == null) return null;

			return Response.HttpContext.Connection.RemoteIpAddress.ToString();
		}


		public static bool Compare(string currentIp, string authIp)
		{
			if (string.IsNullOrEmpty(currentIp) || string.IsNullOrEmpty(authIp)) return false;

			var split = authIp.Split('.');
			if (split.Length != 4) return false;

			return currentIp.StartsWith(split[0] + "." + split[1]);
		}

	}
}
