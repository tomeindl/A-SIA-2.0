using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace A_SIA2WebAPI.BL.PluginSystem
{
    public class PluginLoader<T>
    {
        private readonly ILogger _logger;
        public List<T> LoadedPlugins { get; private set; }

        public PluginLoader(ILogger<PluginLoader<T>> logger)
        {
            _logger = logger;
            LoadedPlugins = new();
        }

        public void LoadPlugins(string pluginFolderPath)
        {
            // Load dlls from folder
            List<string> pluginPaths = Directory.GetFiles(pluginFolderPath).ToList();

            if(pluginPaths.Count == 0)
            {
                _logger.LogInformation("No Plugin dlls were provided");
                return;
            }

            foreach (var file in pluginPaths)
            {
                // Remove non dlls
                if (!file.Contains(".dll"))
                    pluginPaths.Remove(file);
            }

            // Load plugins and add to plugin list
            LoadedPlugins.AddRange(pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateTypeInstances(pluginAssembly);
            }));

            // Display new plugins
            if (LoadedPlugins.Count > 0)
            {
                using (_logger.BeginScope($"Loaded Plugins of type {typeof(T).Name}"))
                {
                    foreach (var plugin in LoadedPlugins)
                    {
                        Console.WriteLine(plugin?.GetType().Name);
                    }
                }
            }
        }

        private IEnumerable<T> CreateTypeInstances(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(T).IsAssignableFrom(type))
                {
                    T? result = (T?)Activator.CreateInstance(type);
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                _logger.LogInformation($"Can't find any type which implements {typeof(T).Name} in {assembly} from {assembly.Location}!");
            }
        }

        private Assembly LoadPlugin(string pluginLocation)
        {
            _logger.LogInformation($"Loaded Plugins from: {Path.GetFileName(pluginLocation)}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }
}
