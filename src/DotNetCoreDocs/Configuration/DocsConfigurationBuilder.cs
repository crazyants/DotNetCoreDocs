using System;
using Microsoft.Extensions.Configuration;

namespace DotNetCoreDocs.Configuration
{
    public static class DocsConfigurationBuilder
    {
        public static DocsConfiguration GetConfiguration(IConfiguration config)
        {
            return new DocsConfiguration() {
                DisplayName = GetValue(config["DocsConfiguration:DisplayName"], "API Documentation"),
                BaseAddress = GetValue(config["DocsConfiguration:BaseAddress"], "http://localhost:5000"),
                RequestsDirectory = GetRequiredValue(config["DocsConfiguration:RequestsDirectory"], "RequestsDirectory"),
                DocumentationRoute = GetValue(config["DocsConfiguration:DocumentationRoute"], "/docs"),
                RootTemplatePath = config["DocsConfiguration:RootTemplatePath"],
                HomeTemplatePath = config["DocsConfiguration:HomeTemplatePath"],
                ModelTemplatePath = config["DocsConfiguration:ModelTemplatePath"],
                ReadmePath = GetValue(config["DocsConfiguration:ReadmePath"], "README.md")
            };
        }

        private static string GetValue(string configValue, string defaultValue)
        {
            if(string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;            
        }

        private static string GetRequiredValue(string configValue, string parameterName)
        {
            if(string.IsNullOrEmpty(configValue))
                throw new ArgumentException($"DocsConfiguration is invalid. {parameterName} is required.");
            return configValue;
        }
    }
}
