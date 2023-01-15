// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using net.hempux.kabuto.Options;
using net.hempux.kabuto.Utilities;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace net.hempux.kabuto
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = LogEventLevel.Information;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .MinimumLevel.Override("Default", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .WriteTo.Console()
                    .CreateLogger();
                levelSwitch.MinimumLevel = LogEventLevel.Information;
            }
            else
            {
                levelSwitch.MinimumLevel = LogEventLevel.Information;
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .MinimumLevel.Override("Default", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                    .WriteTo.Console()
                    .CreateLogger();
            }

            if (!Utils.InDocker && !(File.Exists(string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "appsettings.json"))))
            {
                Log.Fatal("Appsettings.json is missing, create the file and restart application.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            if (Utils.InDocker)
            {

                var Cfg = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();
                AppOptions.Initialize(Cfg);
            }
            else
            {
                try
                {

                    var Cfg = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("appsettings.json", optional: false)
                                   .Build();
                    AppOptions.Initialize(Cfg);
                }
                catch (InvalidDataException)
                {
                    Log.Fatal("The appsettings.json is improperly formated.");
                    Console.ReadKey();
                    Environment.Exit(1);

                }
            }



            Log.Information("Starting server listening on {AppUrl}", AppOptions.ListenAddress);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                levelSwitch.MinimumLevel = LogEventLevel.Warning;

            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();


        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(AppOptions.ListenAddress);
                });
    }

}
