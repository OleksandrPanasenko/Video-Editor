using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoEditor.Core;

//With ChatGPT
namespace VideoEditor.Infrastructure
{
    public static class ProjectStorage
    {
        public static void Save(Project project, string path)
        {
            project.Path = path;
            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            string file_path = Path.Combine(path, project.Name+".vep");
            File.WriteAllText(file_path, json);
        }

        /*public static void SaveAs(Project project, string path)
        {
            project.Path=path;
            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(path, json);
        }*/

        public static Project Load(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);

            var json = File.ReadAllText(path);
            Project project= JsonSerializer.Deserialize<Project>(json);
            if (project == null) throw new FileNotFoundException();
            //project.Path = path;
            //To avoid endless serialization
            project.SelectionManager.Project= project;
            return project;
        }

        public static Project Create(string path, string name)
        {
            string dir_path=Path.Combine(path, name);
            Project project = new Project();
            System.IO.Directory.CreateDirectory(dir_path);
            System.IO.Directory.CreateDirectory(Path.Combine(dir_path, "autosaves"));
            System.IO.Directory.CreateDirectory(Path.Combine(dir_path, "cache"));
            System.IO.Directory.CreateDirectory(Path.Combine(dir_path, "media"));

            project.Path = dir_path;
            project.Name = name;
            Save(project, dir_path);

            return project;

        }
    }
}