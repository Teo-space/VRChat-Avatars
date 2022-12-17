using System.Security.Cryptography;
using Utils.Serialization;

namespace Utils.Crypto
{
	internal static class RSA
	{
		private readonly static string RSAKeyXml = "";

		public static RSAParameters RSAKey = Xml.FromString<RSAParameters>(RSAKeyXml);



		public static byte[] Decrypt(byte[] bytes, RSAParameters privateKey)
		{
			if (bytes == null || bytes.Length == 0)
			{
				throw new ArgumentNullException("byte[] bytes");
			}
			if (ReferenceEquals(privateKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters privateKey");
			}

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(privateKey);
			return csp.Decrypt(bytes, false);
		}


		public static byte[] DecryptBase64(string data, RSAParameters privateKey)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("string data");
			}
			if (ReferenceEquals(privateKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters privateKey");
			}

			var bytes = Base64.BytesFrom(data);

			return Decrypt(bytes, privateKey);
		}



		public static string DecryptBase64ToString(string data, RSAParameters privateKey)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("string data");
			}
			if (ReferenceEquals(privateKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters privateKey");
			}

			var bytes = Base64.BytesFrom(data);

			var result = Decrypt(bytes, privateKey);

			return Encoding.GetString(result);
		}

		public static string DecryptBase64ToString(string data) => DecryptBase64ToString(data, RSAKey);





		public static byte[] Encrypt(byte[] bytes, RSAParameters publicKey)
		{
			if (bytes == null || bytes.Length == 0)
			{
				throw new ArgumentNullException("byte[] bytes");
			}
			if (ReferenceEquals(publicKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters publicKey");
			}

			var csp = new RSACryptoServiceProvider();
			csp.ImportParameters(publicKey);
			return csp.Encrypt(bytes, false);
		}

		public static string EncryptToBase64(byte[] bytes, RSAParameters publicKey)
		{
			return Base64.To(Encrypt(bytes, publicKey));
		}

		public static byte[] EncryptString(string data, RSAParameters publicKey)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("string data");
			}
			if (ReferenceEquals(publicKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters publicKey");
			}

			var bytes = Encoding.GetBytes(data);
			return Encrypt(bytes, publicKey);
		}

		public static string EncryptStringToBase64(string data, RSAParameters publicKey)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("string data");
			}
			if (ReferenceEquals(publicKey, default(RSAParameters)))
			{
				throw new ArgumentNullException("RSAParameters publicKey");
			}

			var bytes = Encoding.GetBytes(data);
			return Base64.To(Encrypt(bytes, publicKey));
		}



	}

}
