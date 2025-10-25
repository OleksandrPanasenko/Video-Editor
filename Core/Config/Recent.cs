using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Config
{
    public class Recent
    {
        public class RecentProject
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public DateTime LastOpened { get; set; }
        }

        public class AppSettings
        {
            public string Theme { get; set; } = "light";
            public string DefaultProjectFolder { get; set; }
            public string FfmpegPath { get; set; }
        }

        public class RecentProjectsList
        {
            public List<RecentProject> Recent { get; set; } = new();
        }
    }
}
