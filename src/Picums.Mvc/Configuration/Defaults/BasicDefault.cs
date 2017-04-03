﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Picums.Mvc.Configuration.Defaults
{
    public abstract class BasicDefault : IDefault
    {
        public void Apply(StartupConfigurations host, IEnumerable<object> arguments)
        {
            host.ASP.Add((app, env) => this.ConfigureApp(app, env, arguments));
            host.Services.Add(services => this.ConfigureServices(services, arguments));
        }

        protected virtual void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<object> arguments)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services, IEnumerable<object> arguments)
        {
        }
    }
}