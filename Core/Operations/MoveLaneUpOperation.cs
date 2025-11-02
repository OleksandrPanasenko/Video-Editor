using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class MoveLaneUpOperation:IOperation
    {
        string IOperation.Name => "MoveLaneUp";
        Project Project;
        Lane Lane;
        int LaneIndex;
        public MoveLaneUpOperation(Project project, Lane lane)
        {
            Project = project;
            Lane = lane;
        }
        public void Apply()
        {
            if (Project != null && Project.Lanes.Contains(Lane))
            {
                LaneIndex = Project.Lanes.IndexOf(Lane);
                Project.RemoveLane(LaneIndex);
                Project.Lanes.Insert(LaneIndex == 0 ? 0 : LaneIndex - 1, Lane);
            }
        }
        public void Undo()
        {
            if (LaneIndex > 0)
            {
                Project.RemoveLane(LaneIndex - 1);
                Project.Lanes.Insert(LaneIndex, Lane);
            }
        }
    }
}
