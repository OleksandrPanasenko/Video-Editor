using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core.Transitions;

namespace VideoEditor.Core.Operations
{
    public class SplitOperation:IOperation
    {
        public string Name => new string("Split");
        private readonly FragmentPlacement First;
        private readonly FragmentPlacement Second;
        private readonly Project Project;
        private readonly Lane Lane;
        private TimeSpan CutOffset;
        ITransition EndTransition;

        public SplitOperation(Project project, Lane lane, FragmentPlacement placement, TimeSpan cutOffset)
        {
            if (placement==null) throw new ArgumentNullException(nameof(placement));
            Project = project;
            Lane = lane;
            First = placement;    
            Second = placement.DeepCopy();
            
            CutOffset = cutOffset;
            EndTransition = Lane.GetTransitionFromTime(placement.EndPosition);
        }
        public void Apply()
        {
            if (EndTransition != null)
            {
                EndTransition.From = Second;
            }
            new TrimOperation(Project, First, First.Fragment.StartTime, CutOffset).Apply();
            new TrimOperation(Project, Second, CutOffset, Second.Fragment.EndTime).Apply();
            Lane.AddFragment(Second, First.EndPosition);
            
        }
        public void Undo()
        {
            Lane.RemoveFragment(Second);
            new TrimOperation(Project, First, First.Fragment.StartTime, Second.Fragment.EndTime).Apply();
            if(EndTransition != null)
            {
                EndTransition.From = First;
            }
        }
    }
}
