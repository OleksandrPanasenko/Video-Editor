using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace VideoEditor.Core
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

        
        
        public class RecentProjectsList
        {
            public List<RecentProject> Recent { get; set; } = new();
            public void AddProject(Project project)
            {
                if (project != null)
                {
                    int i = 0;
                    while (i < Recent.Count)
                    {
                        var recent= Recent[i];
                        {
                            if (project.Path == recent.Path)
                            {
                                Recent.Remove(recent);
                            }
                            else
                            {
                                i++;
                            }
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

    
