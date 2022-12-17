using Newtonsoft.Json;

namespace Utils.Serialization
{
	internal static class Json
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
}
