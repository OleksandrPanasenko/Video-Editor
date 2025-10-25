using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Core.Config.Recent;

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
            if (!File.Exists(SettingsPath)) return new AppSettings();
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json);
        }

        public static void SaveSettings(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }


    }
}
