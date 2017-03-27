﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Picums.Data.Claims;

namespace Picums.Mvc.Startup
{
	public static class AuthenticationBootstrapper
	{
		public static IConfiguration AddAuthentication<TUser, TUserStore>(this IConfiguration startup)
			where TUser : class, IClaimsProvider, IUser
			where TUserStore : class, IUserStore<TUser>
		{
			startup.AddConfiguration((app, e, lf) => app.UseIdentity());
			startup.AddServices(services
				=> services
					.AddClaimsIdentity<TUser>()
					.AddScoped<IUserStore<TUser>, TUserStore>());
			return startup;
		}

		public static IConfiguration ConfigureIdentityOptions(
				this IConfiguration startup
				, string loginPath = "/Login"
				, string logoutPath = "/Logout"
				, string accessDeniedPath = "/Error/RestrictedAccess"
				, int requiredLenght = 8
				, bool slidingExpiration = true
				, bool autmationChallange = true)
			=> startup.ConfigureOption<IdentityOptions>(
				SetIdentityOptions(
					loginPath
					, logoutPath
					, accessDeniedPath
					, requiredLenght
					, slidingExpiration
					, autmationChallange));

		public static IConfiguration ConfigurePasswordHasher(this IConfiguration startup)
			=> startup.ConfigureOption<PasswordHasherOptions>(options =>
			{
				options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
				options.IterationCount = 10000;
			});

		private static Action<IdentityOptions> SetIdentityOptions(
			   string loginPath = "/Login"
			   , string logoutPath = "/Logout"
			   , string accessDeniedPath = "/Error/RestrictedAccess"
			   , int requiredLenght = 8
			   , bool slidingExpiration = true
			   , bool autmationChallange = true)
			=> new Action<IdentityOptions>(options =>
			   {
				   options.Password.RequiredLength = requiredLenght;
				   options.Cookies.ApplicationCookie.LoginPath = new PathString(loginPath);
				   options.Cookies.ApplicationCookie.LogoutPath = new PathString(logoutPath);
				   options.Cookies.ApplicationCookie.AccessDeniedPath = new PathString(accessDeniedPath);
				   options.Cookies.ApplicationCookie.SlidingExpiration = slidingExpiration;
				   options.Cookies.ApplicationCookie.AutomaticChallenge = autmationChallange;
			   });
	}
}