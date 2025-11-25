using Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core.Transitions;

namespace VideoEditor.Core.Operations
{
    public class AddTransitionOperation : IOperation
    {
        Project Project;
        Lane Lane;
        FragmentPlacement A;
        FragmentPlacement B;
        ITransition Transition;
        public string Name => "Add Transition";
        public AddTransitionOperation(Project project, Lane lane, FragmentPlacement a, FragmentPlacement b,ITransition transition)
        {
            if (a.EndPosition != b.Position)
            {
                if (a.Position != b.EndPosition)
                {
                    throw new InvalidOperationException("Fragments are not adjacent");
                }
                else
                {
                    //swap
                    var temp = a;
                    a = b;
                    b = temp;
                }
            }

            Project=project;
            Lane= lane;
            if(transition.Duration > a.Fragment.Duration)
            {
                transition.Duration=a.Fragment.Duration;
            }
            if(transition.Duration > b.Fragment.Duration)
            {
                transition.Duration=b.Fragment.Duration;
            }
            transition.From = a;
            transition.To = b;
            Transition= transition;
            A= a;
            B= b;
        }
        public void Apply()
        {
            
            A.Fragment.EndTime=A.Fragment.EndTime-Transition.Duration;
            B.Fragment.StartTime=B.Fragment.StartTime+Transition.Duration;
            Lane.Transitions.Add(Transition);
        }

        public void Undo()
        {
            Lane.Transitions.Remove(Transition);
            A.Fragment.EndTime = A.Fragment.EndTime + Transition.Duration;
            B.Fragment.StartTime = B.Fragment.StartTime - Transition.Duration;
        }
    }
}
