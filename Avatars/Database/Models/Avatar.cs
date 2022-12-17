namespace Database.Models
{
	public class Avatar
	{
		[Key]
		public string id { get; set; }
		public string name { get; set; }


		public string imageUrl { get; set; }
		public string thumbnailImageUrl { get; set; }
		public string assetUrl { get; set; }


		public string authorName { get; set; }
		public string authorId { get; set; }

		public string unityVersion { get; set; }
	}
}
