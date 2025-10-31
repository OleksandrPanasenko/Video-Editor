using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Operations
{
    public class DoubleLaneOperation : IOperation
    {
        public string Name => "Doublee Lane";
        public Lane NewLane;
        public int NewIndex;
        Project Project;

        public DoubleLaneOperation(Project project, Lane lane)
        {
            Project = project;
            NewLane = lane.DeepCopy();
            NewIndex=project.Lanes.IndexOf(lane)+1;
        }
        public void Apply()
        {
            Project.Lanes.Insert(NewIndex,NewLane);
        }

        public void Undo()
        {
            Project.RemoveLane(NewIndex);
        }
    }
}
