namespace Models.Input
{
	public class AvatarName
	{
		[Required]
		[MinLength(2)]
		[MaxLength(40)]
		public string Name { get; set; }
	}
}
