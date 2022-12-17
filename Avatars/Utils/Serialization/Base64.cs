namespace Utils.Serialization
{
	internal static class Base64
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

}
