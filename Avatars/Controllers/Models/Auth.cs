namespace Avatars.Controllers.Models
{
	public class Auth
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public string userid { get; set; }
		public string name { get; set; }
		public string hwid { get; set; }


		public string pc { get; set; }
		public string cpu { get; set; }
		public string gpu { get; set; }
		public string volume { get; set; }



		public bool NotValid()
		{
			if (ValidationUtils.InvalidLoginOrPassword(Login))
			{
				return true;
			}
			if (ValidationUtils.InvalidLoginOrPassword(Password))
			{
				return true;
			}
			if (string.IsNullOrEmpty(name))
			{
				return true;
			}
			if (ValidationUtils.InvalidUserId(userid))
			{
				return true;
			}
			if (hwid.Length > 128)
			{
				return true;
			}

			if (pc == null) pc = string.Empty;
			if (pc.Length > 128)
			{
				return true;
			}

			if (cpu == null) cpu = string.Empty;
			if (cpu.Length > 128)
			{
				return true;
			}

			if (gpu == null) gpu = string.Empty;
			if (gpu.Length > 128)
			{
				return true;
			}

			if (volume == null) volume = string.Empty;
			if (volume.Length > 128)
			{
				return true;
			}

			return false;
		}




	}


}
