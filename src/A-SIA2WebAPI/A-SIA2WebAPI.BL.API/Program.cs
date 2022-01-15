using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using A_SIA2WebAPI.BL.CalculationSystem;
using A_SIA2WebAPI.BL.CalculationSystem.Roles;
using A_SIA2WebAPI.BL.PluginSystem;
using System.IO;

namespace A_SIA2WebAPI.BL.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // -------------------------
            //          LOGGING
            // -------------------------
            ILoggerFactory loggerFactory = LoggerFactory.Create(logger =>
            {
                logger.ClearProviders();
                logger.AddConsole();
#if DEBUG
                logger.SetMinimumLevel(LogLevel.Debug);
#else
                    logger.SetMinimumLevel(LogLevel.Information);
#endif
            }
            );

            // -------------------------
            //          PLUGINS
            // -------------------------

            // Get path of exe
            string? exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (exeFolder == null)
                throw new ApplicationException("Could not get execution path!");

            // Construct plugin folder path
            string pluginFolder = exeFolder.Replace('\\', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "plugins";

            // Init plugin loaders
            PluginLoader<ICalculator> calculationPluginLoader = new(
                loggerFactory.CreateLogger<PluginLoader<ICalculator>>());

            // Create plugin folder if it does not exist
            if (!Directory.Exists(pluginFolder))
            {
                Directory.CreateDirectory(pluginFolder);
            }
            // Load plugins
            else
            {
                calculationPluginLoader.LoadPlugins(pluginFolder);
            }

            // -------------------------
            //        CALCULATION
            // -------------------------

            CalculationEngine calculationEngine = new();

            // Add plugins to calculation engine
            calculationEngine.AddCalculatorRange(calculationPluginLoader.LoadedPlugins);

            // Add built in calculators
            calculationEngine.AddCalculator(new TransmitterCalculator());
            calculationEngine.AddCalculator(new IsolatorCalculator());

            // -------------------------
            //         DATABASE
            // -------------------------

            // Init db connection here and set database singleton?

            // -------------------------
            //         REST API
            // -------------------------

            CreateHostBuilder(args).Build().Run();
        }

        // Host builder for creating web app
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}