using Avatars.Controllers.Models;
using Avatars.Database.Models;
using Avatars.Database.Repositories.Interfaces;
using Avatars.Utils.Http;
using Database.Models;
using Database.Repositories.Interfaces;
using DbContexts;
using Microsoft.AspNetCore.Http;

namespace Avatars.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserDbContext userDbContext;
		private readonly IUserRepository userRepository;
		public AuthService(IUserDbContext userDbContext, IUserRepository userRepository)
		{
			this.userDbContext = userDbContext;
			this.userRepository = userRepository;
		}


		public async Task<User> Authentificate(HttpRequest Request)
		{
			AuthData authData = Extract(Request);

			User USER = await userRepository.Get(authData.Auth.Login);
			if (USER == null)
			{
				USER = Create(authData.Auth, authData.AesKey, Request.Forwarded());

				await userRepository.Insert(USER);
				await AddAuthLog(authData.Auth);

				return USER;
			}
			else
			{
				if (USER.password != SHA.SHA512(authData.Auth.Password))
				{
					//return JsonMessageCrt.Result(false, "password does not match");
					throw new AuthException("password does not match");
				}
				if (USER.banned)
				{
					//return JsonMessageCrt.Result(false, "banned");
					throw new AuthException("banned");
				}

				USER = Update(USER, authData.Auth, authData.AesKey, Request.Forwarded());

				await userRepository.Update(USER);
				await AddAuthLog(authData.Auth);

				return USER;
			}

		}



		class AuthData
		{
			public Auth Auth;
			public string AesKey;

			public AuthData(Auth auth, string aesKey)
			{
				Auth = auth;
				AesKey = aesKey;
			}
		}

		AuthData Extract(HttpRequest Request)
		{
			if (HttpRequestUtils.WrongUserAgent(Request))
			{
				//return JsonMessageCrt.Result(false, "Access denied(0)");
				throw new AuthException("Access denied(0)");
			}

			string HKey = HttpRequestUtils.Header(Request, "Key");
			if (string.IsNullOrEmpty(HKey))
			{
				//return JsonMessageCrt.Result(false, "HKey is null or empty");
				throw new AuthException("HKey is null or empty");
			}
			string AesKey = RSA.DecryptBase64ToString(HKey);
			if (string.IsNullOrEmpty(AesKey) || AesKey.Length != 32)
			{
				//return JsonMessageCrt.Result(false, "Key invalid");
				throw new AuthException("Key invalid");
			}

			string HeaderAuth = HttpRequestUtils.Header(Request, "Auth");
			if (string.IsNullOrEmpty(HeaderAuth))
			{
				//return JsonMessageCrt.Result(false, "HAuth is null or empty");
				throw new AuthException("HAuth is null or empty");
			}

			string JAuth = Aes.DecryptFromBase64ToString(HeaderAuth, AesKey);
			if (string.IsNullOrEmpty(JAuth))
			{
				//return JsonMessageCrt.Result(false, "JAuth is null or empty");
				throw new AuthException("JAuth is null or empty");
			}

			//var Auth =  Newtonsoft.Json.JsonConvert.DeserializeObject<Auth>(JAuth);
			Auth Auth = System.Text.Json.JsonSerializer.Deserialize<Auth>(JAuth);
			if (Auth == default || Auth.NotValid())
			{
				//return JsonMessageCrt.Result(false, "invalid Auth");
				throw new AuthException("invalid Auth");
			}
			return new AuthData(Auth, AesKey);
		}



		string AuthToken(Auth Auth) => SHA.SHA256(Auth.Login + Auth.Password + Auth.userid + Auth.name + Auth.hwid + DateTime.Now.ToString());

		User Create(Auth Auth, string AesKey, string Ip)
		{
			return new User()
			{
				login = Auth.Login,
				name = Auth.name,

				userid = Auth.userid,
				useridHash = SHA.SHA256(Auth.userid),

				banned = false,

				created = DateTime.Now,
				logged = DateTime.Now,
				last_access = DateTime.Now,


				password = SHA.SHA512(Auth.Password),
				authToken = AuthToken(Auth),

				lastAes = AesKey,

				

				hwid = Auth.hwid,
				ip = Ip,
				settings = "{}",
			};
		}

		User Update(User USER, Auth Auth, string AesKey, string Ip)
		{
			USER.name = Auth.name;
			
			USER.logged = DateTime.Now;
			USER.last_access = DateTime.Now;

			USER.userid = Auth.userid;
			USER.useridHash = SHA.SHA256(Auth.userid);

			USER.authToken = AuthToken(Auth);
			USER.lastAes = AesKey;
			USER.hwid = Auth.hwid;
			USER.ip = Ip;

			return USER;
		}



		async Task AddAuthLog(Auth auth)
		{
			AuthLog authLog = new AuthLog();

			authLog.id = auth.Login + " " + DateTime.Now;
			authLog.login = auth.Login;
			authLog.Time = DateTime.Now;

			authLog.ip = "";

			authLog.userid = auth.userid;
			authLog.hwid = auth.hwid;

			authLog.pc = auth.pc;
			authLog.cpu = auth.cpu;
			authLog.gpu = auth.gpu;
			authLog.volume = auth.volume;

			await userDbContext.AuthLogs.AddAsync(authLog);
			await userDbContext.SaveChangesAsync();
		}
	}







	class AuthException : Exception
	{
		public AuthException(string message): base(message)
		{
		}
	}

}
