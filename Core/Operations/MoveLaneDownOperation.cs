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
        Project project;
        Lane lane;
        int laneIndex;
        public MoveLaneDownOperation(Project _project, Lane _lane) { 
            project = _project;
            lane = _lane;
        }

        

        public void Apply()
        {
            if (project != null && project.Lanes.Contains(lane))
            {
                laneIndex= project.Lanes.IndexOf(lane);
                project.RemoveLane(laneIndex);
                project.Lanes.Insert(laneIndex== project.Lanes.Count - 1 ? project.Lanes.Count - 1 :laneIndex +1, lane);
            }
        }
        public void Undo()
        {
            if (laneIndex < project.Lanes.Count-1) { 
                project.RemoveLane(laneIndex+1);
                project.Lanes.Insert(laneIndex,lane);
            }
        }
    }
}
