using Models;
using Utils;

namespace Database.Models
{
	/// <summary>
	/// AvatarValidator
	/// </summary>
	public class AvatarValidator : AbstractValidator<Avatar>
	{
		/// <summary>
		/// .ctor
		/// </summary>
		public AvatarValidator()
		{
			RuleFor(p => p.id).NotEmpty().Length(41).Matches(Regexes.avatarid);
			RuleFor(p => p.name).NotEmpty().Length(1, 50);
			RuleFor(p => p.imageUrl).NotEmpty().Length(50, 120);
			RuleFor(p => p.thumbnailImageUrl).NotEmpty().Length(50, 120);
			RuleFor(p => p.assetUrl).NotEmpty().Length(50, 130);
			RuleFor(p => p.authorName).NotEmpty().Length(2, 50);
			RuleFor(p => p.authorId).NotEmpty().Length(40).Matches(Regexes.userid);
			RuleFor(p => p.unityVersion).NotEmpty().Length(1, 40);
		}
	}




}
