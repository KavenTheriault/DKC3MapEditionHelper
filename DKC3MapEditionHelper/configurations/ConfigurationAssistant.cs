using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DKC3MapEditionHelper.configurations
{
    public class ConfigurationAssistant
    {
        public AppSettings AppSettings { get; }
        public List<Map> Maps { get; }

        public ConfigurationAssistant()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            Maps = new List<Map>();
            AppSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            foreach (var mapConfiguration in configuration.GetSection("Maps").GetChildren())
            {
                Maps.Add(mapConfiguration.Get<Map>());
            }
        }
    }
}