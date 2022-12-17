namespace Avatars.Database.Models
{
	public class AuthLog
	{
		public string id { get; set; }
		public string login { get; set; }
		public DateTime Time { get; set; }
		public string ip { get; set; }
		public string userid { get; set; }
		public string hwid { get; set; }
		public string pc { get; set; }
		public string cpu { get; set; }
		public string gpu { get; set; }
		public string volume { get; set; }

	}

}
