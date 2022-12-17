namespace Models.Input
{
	public class SingleByField
	{
		[Required]
		[MinLength(2)]
		[MaxLength(25)]
		public string FieldName { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(200)]
		public string FieldValue { get; set; }

	}
}
