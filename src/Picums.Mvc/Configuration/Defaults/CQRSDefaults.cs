﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Picums.Data.CQRS;
using Picums.Data.CQRS.Commands;
using Picums.Data.CQRS.DataAccess;

namespace Picums.Mvc.Configuration.Defaults
{
    public sealed class CQRSDefaults : IDefault
    {
        public void Apply(StartupConfigurations host, IEnumerable<object> arguments)
        {
            arguments
                .Cast<string>()
                .Select(assemblyName => Assembly.Load(new AssemblyName(assemblyName)))
                .ToList()
                .ForEach(assembly => host.Services.Add(AutomaticDetection(assembly)));

            host.Services.Add(services => services.TryAddSingleton<ICommandBus, CommandBus>());
        }

        private Action<IServiceCollection> AutomaticDetection(Assembly assembly)
            => services => services
                .DiscoverImplementation()
                    .ForAssembly(assembly)
                    .IncludeClassesOnly()
                    .ForTypesImplementingInterface<IDataContext>()
                    .AddAsInterface<IDataContext>()
                .DiscoverImplementation()
                    .ForAssembly(assembly)
                    .IncludeClassesOnly()
                    .ForTypesImplementingInterface<IDataContextInitialiser>()
                    .AddAsInterface<IDataContextInitialiser>()
                .DiscoverImplementation()
                    .ForAssembly(assembly)
                    .IncludeClassesOnly()
                    .ForTypesImplementingInterface<ICommandHandler>()
                    .AddAsInterface<ICommandHandler>()
                .DiscoverImplementation()
                    .ForAssembly(assembly)
                    .IncludeClassesOnly()
                    .ForTypesImplementingInterface<IsQuery>()
                    .AsSelf()
                .DiscoverImplementation()
                    .ForAssembly(assembly)
                    .IncludeClassesOnly()
                    .ForTypesImplementingInterface<IStory>()
                    .AsImplementedInterfaces();
    }
}