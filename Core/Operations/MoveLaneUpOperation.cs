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
        Project project;
        Lane lane;
        int laneIndex;
        public MoveLaneUpOperation()
        {

        }
        public void Apply()
        {
            if (project != null && project.Lanes.Contains(lane))
            {
                laneIndex = project.Lanes.IndexOf(lane);
                project.RemoveLane(laneIndex);
                project.Lanes.Insert(laneIndex == 0 ? 0 : laneIndex - 1, lane);
            }
        }
        public void Undo()
        {
            if (laneIndex > 0)
            {
                project.RemoveLane(laneIndex - 1);
                project.Lanes.Insert(laneIndex, lane);
            }
        }
    }
}
