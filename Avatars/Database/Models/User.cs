namespace Database.Models
{
	public class User
	{
		public string login { get; set; }
		public string name { get; set; }
		public string userid { get; set; }

		public bool banned { get; set; }

		public DateTime created { get; set; }
		public DateTime logged { get; set; }
		public DateTime last_access { get; set; }



		public string password { get; set; }
		public string authToken { get; set; }
		public string lastAes { get; set; }
		public string useridHash { get; set; }



		public string hwid { get; set; }




		public string ip { get; set; }

		public string settings { get; set; }

	}


}
