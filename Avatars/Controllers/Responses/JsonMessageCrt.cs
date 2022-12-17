using Microsoft.AspNetCore.Mvc;

namespace Avatars.Controllers.Responses
{
	public class JsonMessageCrt
	{
		public bool Success { get; set; }
		public string Info { get; set; }
		public string crt { get; set; }






		public static JsonResult Result(bool success, string info)
		{
			return new JsonResult(new JsonMessageCrt()
			{
				Success = success,
				Info = info,
				crt = ""
			});
		}

		public static JsonResult Result(bool success, string info, string aesKey)
		{
			string crt = HMAC.SHA256toBase64(info, aesKey);

			var result = new JsonMessageCrt();
			result.Success = success;
			result.Info = info;
			result.crt = crt;

			return new JsonResult(result);
		}

		public static JsonResult ResultAes(bool success, string info, string aesKey)
		{
			return Result(success, Aes.EncryptFromStringToBase64(info, aesKey), aesKey);
		}

	}
}
