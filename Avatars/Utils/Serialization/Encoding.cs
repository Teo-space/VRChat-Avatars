using System;

namespace Utils.Serialization
{
	internal static class Encoding
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



}
