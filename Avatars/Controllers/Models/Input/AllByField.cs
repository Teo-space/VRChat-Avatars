namespace Models.Input
{
	public class AllByField
	{
		[Required]
		[MinLength(2)]
		[MaxLength(25)]
		public string FieldName { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(200)]
		public string FieldValue { get; set; }

		[Required]
		[Range(0, 10000)]
		public int Index { get; set; }
	}
}
