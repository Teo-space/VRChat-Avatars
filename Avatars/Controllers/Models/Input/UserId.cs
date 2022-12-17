using Utils;

namespace Models.Input
{
	public class UserId
	{
		[Required]
		[StringLength(40)]
		[RegularExpression(Regexes.userid, ErrorMessage = "Id must be like usr_00000000-0000-0000-0000-000000000000")]
		public string Id { get; set; }
	}
}
