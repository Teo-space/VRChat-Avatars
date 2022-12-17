using System.Xml;
using System.Xml.Serialization;

namespace Utils.Serialization
{
	internal static class Xml
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

}
