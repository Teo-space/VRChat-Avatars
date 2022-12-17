using System.Security.Cryptography;
using Utils.Serialization;

namespace Utils.Crypto
{
	internal static class HMAC
	{

		public static byte[] SHA256(string data, string key)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("string data");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("string key");
			}

			var bytes = Encoding.GetBytes(data);
			var byteKey = Encoding.GetBytes(key);


			var hmac = new HMACSHA256(byteKey);
			return hmac.ComputeHash(bytes);
		}

		public static string SHA256toBase64(string data, string key)
		{
			var bytes = SHA256(data, key);

			return Base64.To(bytes);
		}



	}

}
