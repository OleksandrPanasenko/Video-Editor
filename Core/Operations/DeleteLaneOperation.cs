using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Operations
{
    public class DeleteLaneOperation : IOperation
    {
        public string Name => new string("AddNewLane");
        Project project;
        private int LaneNumber;
        private Lane Lane;
        public DeleteLaneOperation(Project Project, Lane lane)
        {
            project = Project;
            Lane = lane;
            if (Lane != null) {
                LaneNumber = Project.Lanes.IndexOf(Lane);
            }
        }
        public void Apply()
        {
            project.RemoveLane(LaneNumber);
        }
        public void Undo()
        {
            project.Lanes.Insert(LaneNumber, Lane);
        }
    }
}
