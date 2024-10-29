using System;
using System.IO;
using System.Text.Json;

namespace Helpers
{
    public class ConfigLoader
    {
        // Generyczna metoda do załadowania konfiguracji z pliku JSON
        public static T LoadSectionFromFile<T>(string configFilePath, string sectionName) where T : class
        {
            if (!File.Exists(configFilePath))
                throw new FileNotFoundException("Configuration file not found", configFilePath);

            string jsonString = File.ReadAllText(configFilePath);

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty(sectionName, out JsonElement section))
                {
                    return JsonSerializer.Deserialize<T>(section.GetRawText());
                }
                else
                {
                    throw new InvalidOperationException($"Section '{sectionName}' not found in the configuration file.");
                }
            }
        }
    }
}