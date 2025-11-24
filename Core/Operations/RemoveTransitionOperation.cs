using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core.Transitions;
namespace VideoEditor.Core.Operations
{
    public class RemoveTransitionOperation : IOperation
    {
        public string Name => "Remove Transition";
        
        Lane Lane;
        ITransition Transition;
        public RemoveTransitionOperation(Lane lane, ITransition transition)
        {
            Lane = lane;
            Transition = transition;
        }
        public void Apply()
        {
            if(Transition != null)
            {
                var existing= Lane[Transition.To.EndPosition, Transition.To.EndPosition + Transition.Duration];
                if (existing != null)
                {
                    //Account from void fragment and 
                    if ((existing.Count == 1 && (existing[0] == Transition.To || existing[0].Position== Transition.To.EndPosition+Transition.Duration)) ||
                        (existing.Count == 2 && (existing[0] == Transition.To && existing[1].Position == Transition.To.EndPosition+Transition.Duration)))
                    throw new InvalidOperationException("Cannot remove transition, next fragment is occupied");

                    
                }
                Lane.Transitions.Remove(Transition);
                Transition.From.Fragment.EndTime = Transition.From.Fragment.EndTime + Transition.Duration;
                Transition.To.Fragment.StartTime = Transition.To.Fragment.StartTime - Transition.Duration;
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
