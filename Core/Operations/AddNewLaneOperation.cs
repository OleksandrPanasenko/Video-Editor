using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Operations
{
    public class AddNewLaneOperation:IOperation
    {
        public string Name => new string("AddNewLane");
        Project project;
        private int AddedLane;
        public AddNewLaneOperation(Project Project) { 
            project = Project;
        }
        public void Apply()
        {
            project.AddLane();
            AddedLane = project.Lanes.Count - 1;
        }
        public void Undo()
        {
            project.RemoveLane(AddedLane);
        }
    }
}
