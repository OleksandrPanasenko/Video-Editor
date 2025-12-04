using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core.Transitions;

namespace VideoEditor.Core.Operations
{
    public class TrimTransitionOperation : IOperation
    {
        public string Name => "Trim Transition";
        public ITransition Transition;
        public TimeSpan OldDuration;
        public TimeSpan NewDuration;
        Lane Lane;
        public TrimTransitionOperation(ITransition transition, TimeSpan newDuration, Lane lane)
        {
            OldDuration = transition.Duration;
            if (newDuration < TimeSpan.Zero)
            {
                newDuration = TimeSpan.Zero;
            }
            NewDuration = newDuration;
            Transition = transition;
            Lane = lane;
            if (NewDuration - OldDuration > transition.To.Fragment.Duration)
            {
                NewDuration= OldDuration + transition.To.Fragment.Duration;
            }
            if (NewDuration - OldDuration > transition.From.Fragment.Duration)
            {
                NewDuration= OldDuration + transition.From.Fragment.Duration;
            }
                
        }
        public void Apply()
        {
            //Add check for end fragment overlapping
            TimeSpan difference =NewDuration - OldDuration;
            if (difference == TimeSpan.Zero) return;
            else
            {
                Transition.Duration = NewDuration;
                Transition.From.Fragment.EndTime = Transition.From.Fragment.EndTime - difference;
                Transition.To.Fragment.StartTime = Transition.To.Fragment.StartTime + difference;
            }
        }

        public void Undo()
        {
            TimeSpan difference = NewDuration - OldDuration;
            if (difference == TimeSpan.Zero) return;
            if (NewDuration < OldDuration)
            {
                Transition.Duration = NewDuration;
                Transition.From.Fragment.EndTime = Transition.From.Fragment.EndTime + difference;
                Transition.To.Fragment.StartTime = Transition.To.Fragment.StartTime - difference;
            }
            else
            {
                Transition.Duration = NewDuration;
                Transition.From.Fragment.EndTime = Transition.From.Fragment.EndTime - difference;
                Transition.To.Fragment.StartTime = Transition.To.Fragment.StartTime + difference;
            }
        }
    }
}
