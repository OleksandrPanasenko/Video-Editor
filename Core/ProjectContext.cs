using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public static class ProjectContext
    {
        public static Project? CurrentProject { get; private set; }

        public static void Open(Project project)
        {
            CurrentProject = project;
        }

        public static void Close()
        {
            CurrentProject = null;
        }
    }
}
