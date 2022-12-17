using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using System.Text.Encodings.Web;

using Utils;
using Avatars.Utils.Http;
using Utils.Crypto;
using Database.Models;
using Database.Repositories.Interfaces;
using Avatars.Services.Interfaces;
using AvatarsApi.Controllers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AuthHandlers
{
	/// <summary>
	/// Custom Auth Scheme
	/// </summary>
	public class CustomAuthSchemeOptions : AuthenticationSchemeOptions
	{
		/// <summary>
		/// Scheme Name
		/// </summary>
		public const string Name = "CustomAuthSchemeOptions";
	}


	/// <summary>
	/// Custom Auth Handler
	/// </summary>
	internal class CustomAuthHandler : AuthenticationHandler<CustomAuthSchemeOptions>
	{
		private readonly ILogger<CustomAuthHandler> logger;
		private readonly IUserRepository userRepository;
		private readonly ICurrentUserServise currentuserServise;

		public CustomAuthHandler
		(
			IOptionsMonitor<CustomAuthSchemeOptions> optionsMonitor,
			ILoggerFactory loggerFactory,
			UrlEncoder encoder,
			ISystemClock clock,
			IUserRepository userRepository,
			ICurrentUserServise currentuserServise
		) : base(optionsMonitor, loggerFactory, encoder, clock)
		{
			this.logger = loggerFactory.CreateLogger<CustomAuthHandler>();
			this.userRepository = userRepository;
			this.currentuserServise = currentuserServise;
		}



		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			try
			{
				Endpoint = $"{Request.Method}-{Request.Path}";
				Ip = Request.Forwarded();


				if (HttpRequestUtils.WrongUserAgent(Request))
				{
					return Fail("Access denied");
				}

				string AesKey = RSA.DecryptBase64ToString(HttpRequestUtils.Header(Request, "Key"));
				if (string.IsNullOrEmpty(AesKey) || AesKey.Length != 32)
				{
					return Fail("Key empty or invalid");
				}

				string AuthToken = Aes.DecryptFromBase64ToString(HttpRequestUtils.Header(Request, "Auth"), AesKey);
				if (string.IsNullOrEmpty(AuthToken) || AuthToken.Length != 64)
				{
					return Fail("Auth Invalid or empty");
				}

				string userid = Aes.DecryptFromBase64ToString(HttpRequestUtils.Header(Request, "user"), AesKey);
				if (ValidationUtils.InvalidUserId(userid))
				{
					return Fail("Invalid Headeruser");
				}

				return await Authuser(AuthToken, AesKey, userid);
			}
			catch (Exception ex)
			{
				return Fail(ex);
			}
		}



		async Task<AuthenticateResult> Authuser(string AuthToken, string AesKey, string userid)
		{
			var user = await userRepository.GetByToken(AuthToken);
			if (user == null)
			{
				return Fail("user not found");
			}
			if (user.lastAes == AesKey)
			{
				return Fail("Access denied (repeat)");
			}
			if (user.userid != userid)
			{
				return Fail($"{user.login}, Multiple Access Denied");
			}
			if (user.logged.AddDays(7) < DateTime.Now)
			{
				return Fail("Your token has expired.");
			}


			user.lastAes = AesKey;
			user.last_access = DateTime.Now;

			//await userRepository.Update(user);

			currentuserServise.Set(user);

			return AuthenticateResult.Success(Ticket(user, AesKey));
		}


		AuthenticationTicket Ticket(User user, string AesKey)
		{
			var claims = new[]
			{
				new Claim("aes", AesKey),

				new Claim("login", user.login),
				new Claim("userid", user.userid),

				new Claim("settings", user.settings),
			};
			var claimsIdentity = new ClaimsIdentity(claims, nameof(CustomAuthHandler));
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			return new AuthenticationTicket(claimsPrincipal, this.Scheme.Name);
		}



		class ErrorMessage
		{
			public string Endpoint { get; set; }
			public string Ip { get; set; }
			public string Error { get; set; }

			public ErrorMessage(string endpoint, string ip, string error)
			{
				Endpoint = endpoint;
				Ip = ip;
				Error = error;
			}
		}


		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			try
			{
				if (Response.HasStarted)
				{
					var errorMessage = new ErrorMessage(Endpoint, Ip, failReason);
					var json = Json.Serealize(errorMessage);
					await Response.WriteAsync(json);
				}
				else
				{
					Context.Response.StatusCode = 401;
					Response.ContentType = "application/json";
					var errorMessage = new ErrorMessage(Endpoint, Ip, failReason);
					var json = Json.Serealize(errorMessage);
					await Response.WriteAsync(json);
				}
			}
			catch (Exception ex)
			{

			}
		}


		string Endpoint;
		string Ip;
		string failReason;



		AuthenticateResult Fail(Exception ex)
		{
			failReason = ex.ToString();
			return AuthenticateResult.Fail($"Endpoint3: {Endpoint}   Ip:{Ip} \n{ex.ToString()}");
		}
		AuthenticateResult Fail(string failureMessage)
		{
			failReason = failureMessage;
			return AuthenticateResult.Fail($"Endpoint3: {Endpoint}   Ip:{Ip} {failureMessage}");
		}

	}
}
