﻿using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui;

namespace MauiController
{
    public class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.ConfigureSyncfusionCore();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        async void OnSettingClicked(object sender)//, EventArgs e)
        {

        }

    }



}
