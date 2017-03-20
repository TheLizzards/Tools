﻿using TheLizzards.Mvc.FeatureSlices;

namespace TheLizzards.Mvc.Configuration.Defaults
{
    public sealed class FeaturesDefaults : IDefault
    {
        public void Apply(StartupConfigurations host)
        {
            host.MVC.Conventions.AddControllerConvention<FeatureConvention>();

            host.Razor.Options(options
                => new ViewLocationFormatsUpdater(options)
                    .UpdateViewLocations()
                    .AddExtender());
        }
    }
}