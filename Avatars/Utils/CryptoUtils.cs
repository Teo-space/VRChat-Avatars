using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace Utils
{


	internal class CryptoUtils
	{
		public static class Encoding
		{
			public readonly static System.Text.Encoding encoding = System.Text.Encoding.UTF8;

			public static byte[] GetBytes(string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				return encoding.GetBytes(data);
			}

			public static string GetString(byte[] bytes)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				return encoding.GetString(bytes);
			}
		}

		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################


		public static class Base64
		{
			public static string To(string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				var bytes = Encoding.GetBytes(data);
				return Convert.ToBase64String(bytes);
			}

			public static string To(byte[] bytes)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				return Convert.ToBase64String(bytes);
			}



			public static string From(string base64string)
			{
				if (string.IsNullOrEmpty(base64string))
				{
					throw new ArgumentNullException("string base64string");
				}
				var bytes = Convert.FromBase64String(base64string);
				return Encoding.GetString(bytes);
			}

			public static byte[] BytesFrom(string base64string)
			{
				if (string.IsNullOrEmpty(base64string))
				{
					throw new ArgumentNullException("string base64string");
				}

				return Convert.FromBase64String(base64string);
			}





		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################



		public static class Json
		{
			public static string Serealize<T>(T o)
			{
				if (ReferenceEquals(o, default(T)))
				{
					throw new ArgumentNullException("T o");
				}

				return JsonConvert.SerializeObject(o);
			}
			public static T Deserealize<T>(string json) where T : class
			{
				if (string.IsNullOrEmpty(json))
				{
					throw new ArgumentNullException("string json");
				}

				return JsonConvert.DeserializeObject<T>(json);
			}
		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################


		public static class Xml
		{
			private static XmlSerializerNamespaces SerializerNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
			private static XmlWriterSettings WriterSettings = new XmlWriterSettings()
			{
				Indent = true,
				OmitXmlDeclaration = true
			};


			public static string ToString<T>(T value, bool Indent = false)
			{
				if (ReferenceEquals(value, default(T)))
				{
					throw new ArgumentNullException("T value");
				}

				using (var stream = new StringWriter())
				{
					WriterSettings.Indent = Indent;
					using (var writer = XmlWriter.Create(stream, WriterSettings))
					{
						var serializer = new XmlSerializer(value.GetType());
						serializer.Serialize(writer, value, SerializerNamespaces);
						return stream.ToString();
					}
				}
			}

			public static T FromString<T>(string xml)
			{
				if (string.IsNullOrEmpty(xml))
				{
					throw new ArgumentNullException("string xml");
				}

				StringReader stringReader = new StringReader(xml);
				var serializer = new XmlSerializer(typeof(T));

				return (T)serializer.Deserialize(stringReader);
			}

		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################


		public static class RSA
		{
			private readonly static string RSAKeyXml = "<RSAParameters><Exponent>AQAB</Exponent><Modulus>nqSX5zQu8OFH5/fMX4G97Y9G7M3m/MmocTG1dge9EJRpwTaUz3otTBVADDWMtVwub4VZbFbJljwJQF/W8Ash842+6o2zud+z2GwZ3SehEP66zxC1F2YyLpSGc7arVFGC9DANUDB5HgK7w1XgUj88+eHWN6rWhtVyxvfTcpJRg9M=</Modulus><P>+fAvvrbcXX/v53oF3agvYWFmQj0MAY2nLlvsJ85PC/Jsr+j1UUa7CC5MunX1RvuRl6lh9GSohtJcz8ziVC4hBw==</P><Q>on2S45zZ5XUSxIe/VEgnQ2YM2lrHDcD57ktBmSjeh+/0PJeyKgm4x39GkAoNpAWcwLjBY0UxfutobtF9efJv1Q==</Q><DP>UB1EQJ5+rl3G+WysmqtBXLaKv6qvwYO1Ve/TF8NSpSK04gILOF0ysGGe6JnM4E7dET8TDfKB7o1ZgKZM2ezbWw==</DP><DQ>nNcMo3rQr7STSvOkcBu9Gkc0fMdGfzYXiDVHuihCs3+fhaT+qaj8nENzvrCVfg6UwUkUEVfGcLWU9fJrZPvniQ==</DQ><InverseQ>AA7F65M68Qyy4s9JP5q3Lv8NdjJCGlohOowJusDloep4aoDJLsGmI36I8GpCIeClE+9vY4XjUNvA9t7BJSBeyQ==</InverseQ><D>T75uuQJrIhwr1/egnNWL0hE2Vqx9ks4PRK73BxiGugHQUxYLoWtJv7fi1QBJ9ZQbp2VBeQr7E/Cjud8zHMCL1KE36TgQsGAnMSz5L3oJ1vKGOM+xsnt2sNLzwafrYZnqUHwhBWDiywMU+6OMSvQNhpdWaS0SBN/3naEqg39pF4k=</D></RSAParameters>";

			public static RSAParameters RSAKey = CryptoUtils.Xml.FromString<RSAParameters>(RSAKeyXml);



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


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################



		public static class Aes
		{
			public static byte[] Encrypt(byte[] bytes, byte[] key)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				if (key == null || key.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				if (key.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}

				using (var aes = System.Security.Cryptography.Aes.Create())
				{
					aes.Key = key;
					aes.GenerateIV();

					ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

					byte[] bytesEncoded = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);

					byte[] result = new byte[aes.IV.Length + bytesEncoded.Length];

					Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
					Array.Copy(bytesEncoded, 0, result, aes.IV.Length, bytesEncoded.Length);

					return result;
				}
			}


			public static string EncryptFromBytesToBase64(byte[] bytes, string key)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("string key");
				}

				var byteKey = Encoding.GetBytes(key);

				if (byteKey.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}

				var encrypted = Encrypt(bytes, byteKey);

				return Base64.To(encrypted);
			}

			public static string EncryptFromStringToBase64(string data, string key)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("string key");
				}

				var byteKey = Encoding.GetBytes(key);
				if (byteKey.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}
				var bytes = Encoding.GetBytes(data);

				var encrypted = Encrypt(bytes, byteKey);

				return Base64.To(encrypted);
			}








			public static byte[] Decrypt(byte[] bytes, byte[] key)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				if (key == null || key.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				if (key.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}

				using (var aes = System.Security.Cryptography.Aes.Create())
				{
					byte[] vector = new byte[aes.BlockSize / 8];
					Array.Copy(bytes, 0, vector, 0, vector.Length);

					byte[] bytesEncoded = new byte[bytes.Length - vector.Length];
					Array.Copy(bytes, vector.Length, bytesEncoded, 0, bytesEncoded.Length);

					aes.Key = key;
					aes.IV = vector;

					ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);

					byte[] result = cryptoTransform.TransformFinalBlock(bytesEncoded, 0, bytesEncoded.Length);

					return result;
				}
			}


			public static string DecryptFromBase64ToString(string data, string key)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("string key");
				}

				var byteKey = Encoding.GetBytes(key);
				if (byteKey.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}

				var bytes = Base64.BytesFrom(data);

				var decrypted = Decrypt(bytes, byteKey);

				return Encoding.GetString(decrypted);
			}

			public static byte[] DecryptFromBase64ToBytes(string data, string key)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				if (string.IsNullOrEmpty(key))
				{
					throw new ArgumentNullException("string key");
				}

				var byteKey = Encoding.GetBytes(key);
				if (byteKey.Length != 32)
				{
					throw new ArgumentException("Key length does not match the algorithm");
				}

				var bytes = Base64.BytesFrom(data);

				return Decrypt(bytes, byteKey);
			}








		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################



		public static class SHA
		{
			static SHA512CryptoServiceProvider SHA512Provider = new SHA512CryptoServiceProvider();
			static SHA256CryptoServiceProvider SHA256Provider = new SHA256CryptoServiceProvider();



			public static string SHA512(string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				return BitConverter.ToString(SHA512Provider.ComputeHash(Encoding.GetBytes(data))).Replace("-", null);
			}


			public static string SHA256(string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				return BitConverter.ToString(SHA256Provider.ComputeHash(Encoding.GetBytes(data))).Replace("-", null);
			}

		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################


		public static class Random
		{
			static System.Random random = new System.Random();

			static char[] Symbols = new char[]
			{
					'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm',

					'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M',

					'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
			};

			private static char RandomSymbol()
			{
				return Symbols[random.Next(0, Symbols.Length)];
			}

			public static string String(int Length)
			{
				string result = "";
				while (result.Length < Length)
				{
					result += RandomSymbol();
				}
				return result;
			}

			public static string RandomNum() => random.Next(int.MaxValue / 2, int.MaxValue - 1).ToString() + random.Next(int.MaxValue / 4, int.MaxValue / 2).ToString();

		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################


		public static class MD5
		{
			static System.Security.Cryptography.MD5 Hash = System.Security.Cryptography.MD5.Create();
			public static string Compute(byte[] bytes)
			{
				if (bytes == null || bytes.Length == 0)
				{
					throw new ArgumentNullException("byte[] bytes");
				}
				return BitConverter.ToString(Hash.ComputeHash(bytes)).Replace("-", null).ToLowerInvariant();
			}

			public static string Compute(string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new ArgumentNullException("string data");
				}
				return Compute(Encoding.GetBytes(data));
			}

		}


		//#################################################################################################################################
		//#################################################################################################################################
		//#################################################################################################################################



		public static class HMAC
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


}
