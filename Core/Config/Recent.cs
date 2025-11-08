using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Config
{
    public static class Recent
    {
        public class RecentProject
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public DateTime LastSaved { get; set; }
            public RecentProject(string name, string path)
            {
                Name = name;
                Path = path;
                LastSaved = DateTime.Now;
            }
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
            public void AddProject(Project project)
            {
                if (project != null)
                {
                    foreach (var recent in Recent)
                    {
                        if (project.Path == recent.Path)
                        {
                            Recent.Remove(recent);
                        }
                    }
                }
                Recent.Insert(0, new RecentProject(project.Name, project.Path));
                while (Recent.Count > 5)
                {
                    Recent.RemoveAt(5);
                }
            }
        }
    }
}
