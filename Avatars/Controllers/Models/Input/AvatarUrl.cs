namespace Models.Input
{
	public class AvatarUrl
	{
		[Required]
		[MinLength(50)]
		[MaxLength(120)]
		public string Url { get; set; }
	}


}
