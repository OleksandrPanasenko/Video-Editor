using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace VideoEditor.Infrastructure
{
    //Generated with ChatGPT
    public static class ConfigManager
    {
        private static readonly string BaseDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoEditor", "Config");

        private static readonly string RecentPath = Path.Combine(BaseDir, "recentProjects.json");
        private static readonly string SettingsPath = Path.Combine(BaseDir, "appsettings.json");

        static ConfigManager()
        {
            Directory.CreateDirectory(BaseDir);
        }

        public static RecentProjectsList LoadRecent()
        {
            if (!File.Exists(RecentPath)) return new RecentProjectsList();
            var json = File.ReadAllText(RecentPath);
            return JsonSerializer.Deserialize<RecentProjectsList>(json);
        }

        public static void SaveRecent(RecentProjectsList list)
        {
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RecentPath, json);
        }

        public static AppSettings LoadSettings()
        {
            //Resolve code not loading issue
            if (!File.Exists(SettingsPath)) return null;
            var options = new JsonSerializerOptions
            {
                Converters = { new ColorJsonConverter() }
            };
            var json = File.ReadAllText(SettingsPath);

            try {
                return JsonSerializer.Deserialize<AppSettings>(json, options);
            }catch(JsonException ex)
            {
                return null;
            }
            catch(InvalidOperationException ex)
            {
                return null;
            }
        }

        public static void SaveSettings(AppSettings settings)
        {
            //Custom save to fix color not loading
            var options = new JsonSerializerOptions
            {
                Converters = { new ColorJsonConverter() },
                WriteIndented = true // Makes the JSON file easier to read
            };

            var json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(SettingsPath, json);
        }


    }
}
