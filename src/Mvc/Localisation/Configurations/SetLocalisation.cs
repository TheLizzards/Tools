﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Picums.Mvc.Localisation;
using Picums.Mvc.Middleware;

namespace Picums.Mvc.Configuration.Defaults
{
    public sealed class SetLocalisation : IDefault
    {
        public void Apply(StartupConfigurations host, IEnumerable<object> arguments)
        {
            var cultureStore = this.GetCultureStore(host.ConfigurationRoot);

            host.Services.Add(x => x
                .AddSingleton(cultureStore)
                .AddSingleton<IdentityErrorDescriber, LocalisedIdentityErrorDescriber>()
                .AddScoped<IStringLocalizer, ConfigurableStringLocalizer>()
                .AddScoped<IHtmlLocalizer, HtmlLocalizer>());

            host.ASP.Add(this.ConfigureRequestLocalisation(cultureStore));

            host.Apply<MiddlewareDefault<CultureCookieSetterMiddleware>>();
        }

        private CultureStore GetCultureStore(IConfigurationRoot configurationRoot)
            => new CultureStore(
                this.GetDefaultLanguage(configurationRoot, $"Culture:Default")
                , this.GetAvailableLanguages(configurationRoot, $"Culture:Available"));

        private CultureInfo GetDefaultLanguage(IConfigurationRoot configurationRoot, string selector)
            => new CultureInfo(configurationRoot[selector]);

        private IEnumerable<CultureInfo> GetAvailableLanguages(IConfigurationRoot configurationRoot, string selector)
            => configurationRoot
                .GetSection(selector)
                .GetChildren()
                .Select(x => x.Value)
                .Select(x => new CultureInfo(x))
                .ToList();

        private Action<IApplicationBuilder, IHostingEnvironment, ILoggerFactory> ConfigureRequestLocalisation(CultureStore cultureStore)
        {
            var defaultLanguage = new RequestCulture(cultureStore.DefaultCulture);
            var availableLanguages = cultureStore.AvailableCultures.ToList();

            return (app, env, lg) => app.UseRequestLocalization(
                   new RequestLocalizationOptions
                   {
                       RequestCultureProviders = new List<IRequestCultureProvider>
                       {
                            new AcceptLanguageHeaderRequestCultureProvider(),
                            new QueryStringRequestCultureProvider(),
                            new CookieRequestCultureProvider(),
                       },
                       SupportedCultures = availableLanguages,
                       SupportedUICultures = availableLanguages,
                       DefaultRequestCulture = defaultLanguage,
                   });

            //        UseMiddlewareCultureRecognition(
            //defaultLanguage
            //,
            //, new CookieRequestCultureProvider()
            //, new QueryStringRequestCultureProvider()
            //, new AcceptLanguageHeaderRequestCultureProvider());
        }
    }
}