using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
	internal class ValidationUtils
	{

		/*
        public static Regex guidRegex = new Regex(@"^\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$");

        public static bool WrongGuid(string guid)
        {
            if (guid.Empty() || guid.Length != 36 || !guidRegex.IsMatch(guid)) return true;
            return false;
        }
		*/



		public static Regex useridRegex = new Regex(@"^usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$");

		public static bool InvalidUserId(string userid)
		{
			if (string.IsNullOrEmpty(userid) || userid.Length != 40 || useridRegex.IsMatch(userid) == false) return true;
			return false;
		}



		/*
        public static Regex worldidRegex = new Regex(@"^wrld_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$");

        public static Regex IdWithTagsPublic = new Regex(@"^[\d{1-5}]+$");

        public static Regex IdWithTagsPrivate = new Regex(@"^[\d{1-5}]+~(hidden|friends|private)\(usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}\)(~nonce|~canRequestInvite~nonce)\(\w{64}\)$");

		public static Regex Full = new Regex(@"^wrld_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}:(\w{1,34}|\w{1,34}~(hidden|friends|private)\(usr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}\)(~nonce|~canRequestInvite~nonce)\((\w{64}|\w{8}-\w{4}-\w{4}-\w{4}-\w{12})\))$");

		public static bool WrongWorld(string LocationWithTags)
        {
            if (LocationWithTags.Empty() || LocationWithTags.Length > 250 || LocationWithTags.Contains(':') == false) return true;

			if (Full.IsMatch(LocationWithTags) == false) return true;

			return false;
        }
		*/




		public static Regex SearchRegex = new Regex(@"^\w{1,25}$");
		public static bool InvalidSearch(string search)
		{
			if (string.IsNullOrEmpty(search) || search.Length < 3 || search.Length > 25 || SearchRegex.IsMatch(search) == false) return true;
			return false;
		}


		public static Regex regexLoginOrPassword = new Regex(@"^\w{8,20}$");
		public static bool InvalidLoginOrPassword(string data)
		{
			if (string.IsNullOrEmpty(data) || data.Length < 8 || data.Length > 21 || !regexLoginOrPassword.IsMatch(data)) return true;
			return false;
		}




		public static Regex avataridRegex = new Regex(@"^avtr_\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$");

		public static bool InvalidAvatarId(string avatarid)
		{
			if (string.IsNullOrEmpty(avatarid) || avatarid.Length != 41 || !avataridRegex.IsMatch(avatarid)) return true;
			return false;
		}






	}

}
