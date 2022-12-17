using Utils;

namespace Models.Input
{
	public class AvatarId
	{
		[Required]
		[StringLength(41)]
		[RegularExpression(Regexes.avatarid, ErrorMessage = "Id must be like avtr_00000000-0000-0000-0000-000000000000")]
		public string Id { get; set; }
	}

	/*
	public class AvatarIdValidator : AbstractValidator<AvatarId>
	{
		public AvatarIdValidator()
		{
			RuleFor(x => x.Id).NotEmpty().Length(41).Matches(avatarid)
				.WithMessage("avatarid must be like avtr_00000000-0000-0000-0000-000000000000");
		}
	}
	*/


}
