using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class MoveLaneDownOperation:IOperation
    {
        string IOperation.Name => "MoveLaneDown";
        Project Project;
        Lane Lane;
        int LaneIndex;
        public MoveLaneDownOperation(Project project, Lane lane) { 
            Project = project;
            Lane = lane;
        }

        

        public void Apply()
        {
            if (Project != null && Project.Lanes.Contains(Lane))
            {
                LaneIndex= Project.Lanes.IndexOf(Lane);
                Project.RemoveLane(LaneIndex);
                Project.Lanes.Insert(LaneIndex== Project.Lanes.Count ? Project.Lanes.Count :LaneIndex +1, Lane);
            }
        }
        public void Undo()
        {
            if (LaneIndex < Project.Lanes.Count-1) { 
                Project.RemoveLane(LaneIndex+1);
                Project.Lanes.Insert(LaneIndex,Lane);
            }
        }
    }
}
