using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using AuthHandlers;
using Services.Interfaces;
using Services;
using Database.Repositories.Interfaces;
using Database.Models;
using Database.Repositories.Frameworks.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Filters;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DbContexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Avatars.Services;
using Avatars.Database.Repositories;
using Avatars.Services.Interfaces;
using Avatars.Database.Repositories.Interfaces;


namespace AvatarsApi
{

	public class Startup
    {
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMemoryCache();
			services.AddLogging();
			services.AddCors();


			services
				.AddAuthentication(options =>{ /*options.DefaultScheme = CustomAuthSchemeOptions.Name;*/ })
				.AddScheme<CustomAuthSchemeOptions, CustomAuthHandler>(CustomAuthSchemeOptions.Name, options =>{});



			services.AddDbContext<UserDbContext>();
			services.AddDbContext<IUserDbContext, UserDbContext>();
			services.TryAddScoped<UserRepository>();
			services.TryAddScoped<IUserRepository, UserRepository>();
			services.TryAddScoped<CurrentUserServise>();
			services.TryAddScoped<ICurrentUserServise, CurrentUserServise>();
			services.TryAddScoped<AuthService>();
			services.TryAddScoped<IAuthService, AuthService>();



			services.AddDbContext<AvatarsDbContext>();
			services.AddDbContext<IAvatarsDbContext, AvatarsDbContext>();


			services.TryAddScoped<FavoriteRepository>();
			services.TryAddScoped<IFavoriteRepository, FavoriteRepository>();

			services.TryAddScoped<FavoriteService>();
			services.TryAddScoped<IFavoriteService, FavoriteService>();

			services.TryAddScoped<AvatarRepository>();
			services.TryAddScoped<IAvatarRepository, AvatarRepository>();

			services.TryAddScoped<AvatarService>();
			services.TryAddScoped<IAvatarService, AvatarService>();



			services.AddControllersWithViews(options =>
			{
				options.Filters.Add(typeof(CustomExceptionFilterAttribute));
				options.Filters.Add(typeof(CounterAttribute));
			})
			.AddFluentValidation(config =>
			{
				config.RegisterValidatorsFromAssemblyContaining<Startup>();
			});





			{
				services
					.AddHealthChecks()
						.AddDbContextCheck<UserDbContext>()
						.AddDbContextCheck<AvatarsDbContext>();

				services
					.AddHealthChecksUI(settings =>
					{
						settings.AddHealthCheckEndpoint("Self Health Checks", "/health");
						settings.SetEvaluationTimeInSeconds(60);
						settings.SetApiMaxActiveRequests(1);
					})
					.AddInMemoryStorage();
			}




			services.AddSwaggerGen();
		}




		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI();
			}


            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();


			app.UseCors(builder =>
			{
				builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
			});


			//route:health
			app.UseHealthChecks("/health", new HealthCheckOptions()
			{
				Predicate = x => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});//.RequireAuthorization();
			app.UseHealthChecksUI();//route:healthchecks-ui


			app.UseEndpoints(endpoints =>
            {
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
			});

			

		}


    }
}

/*

 {

	"Connection":["Keep-Alive"],

	"Content-Type":["application/x-www-form-urlencoded"],

	"Accept":["*//*"],
	              * 
	"Accept-Encoding":["gzip"],

	"Host":["avatars.iteo.space"],

	"User-Agent":["UnityPlayer/2019.4.31f1 (UnityWebRequest/1.0, libcurl/7.75.0-DEV)"],
	"Content-Length":["0"],
	"cf-ipcountry":["RU"],
	"cdn-loop":["cloudflare"],
	"x-forwarded-for":["5.227.24.2"],
	"cf-ray":["72e34a253c7bfa30-AMS"],
	"x-forwarded-proto":["https"],
	"cf-visitor":["{\"scheme\":\"https\"}"],
	"key":["KtZ53r9m+Q+ttp1ybI8S2L8atXXvvgswK5StmW9/scncsu/XESkYWkDytGl7exrU19KcKap80Ehzi9t5spqU3yO9mhhWv+0RqJbRfkHw76EysRTwB+0P/2ZLRQpVOnHIZmP0wi7QtYRdNcJOBQCrRtGhGHxUaZt9ALTb1ka4oCo="],
	"auth":["hCPUCtlpu7UP9fTQCJOrDZnYJuAEaH7QACwlEAzOYkgCWHvPCIxOONQZ94wocTr2uLkTIBIELNcLzPUDrsSImG49G1v/CA5QqVQ/ekQrXP6GCfp5X9DnTdXUAwDlI7hi"],
	"user":["KcQumHqkGB1F2Yx5InR2d6ZlfinfSAzNLWDZtFRC21jywSCOVwQXk47iTPWe5C2BC6Jni8yf7OeoMhH9IXYBSw=="],
	"x-unity-version":["2019.4.31f1"],
	
	"cf-connecting-ip":["5.227.24.2"],
	"MS-ASPNETCORE-TOKEN":["0a6839fc-e05f-4ddb-9aa3-707c274a47cc"],
	"X-Original-For":["127.0.0.1:54688"],
	"X-Original-Proto":["http"]
}
 
 
*/