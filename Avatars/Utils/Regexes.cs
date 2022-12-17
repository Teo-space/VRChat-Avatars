namespace Utils
{
	internal static class Regexes
	{
		public const string Guid = @"\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}";

		public const string userid = @"^usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$";
		public const string avatarid = @"^avtr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$";

		public const string worldid = @"^wrld_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$";



		/*
		public static Regex worldidRegex = new Regex(@"^wrld_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$");

		public static Regex IdWithTagsPublic = new Regex(@"^[\d{1-5}]+$");

		public static Regex IdWithTagsPrivate = 
		new Regex(@"^[\d{1-5}]+~(hidden|friends|private)\(usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}\)(~nonce|~canRequestInvite~nonce)\(\w{64}\)$");

		public static Regex Full = 
		new Regex(@"^wrld_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}:(\w{1,34}|\w{1,34}~(hidden|friends|private)\(usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}\)(~nonce|~canRequestInvite~nonce)\((\w{64}|\w{8}-\w{4}-\w{4}-\w{4}-\w{12})\))$");

		public static bool WrongWorld(string LocationWithTags)
		{
			if (LocationWithTags.Empty() || LocationWithTags.Length > 250 || LocationWithTags.Contains(':') == false) return true;

			if (Full.IsMatch(LocationWithTags) == false) return true;

			return false;
		}
		*/
	}
}
