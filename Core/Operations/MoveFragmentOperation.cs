using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Operations
{
    public class MoveFragmentOperation : IOperation
    {
        public string Name => "MoveFragment";
        public Lane OldLane;
        public Lane NewLane;

        public TimeSpan OldTimeStart;
        public TimeSpan NewTimeStart;

        FragmentPlacement FragmentPlacement;

        public MoveFragmentOperation(Lane oldLane, Lane newLane, TimeSpan oldTime, TimeSpan newTime)
        {
            OldLane = oldLane;
            NewLane = newLane;
            if (oldLane[oldTime] == null)
            {
                throw new ArgumentNullException("No selected fragment");
            }
            FragmentPlacement = oldLane[oldTime];

            OldTimeStart = FragmentPlacement.Position;
            NewTimeStart = newTime-(oldTime-OldTimeStart);
            
        }
        public void Apply()
        {
            OldLane.RemoveFragment(FragmentPlacement);
            NewLane.AddFragment(FragmentPlacement,NewTimeStart);
        }

        public void Undo()
        {
            NewLane.RemoveFragment(FragmentPlacement);
            OldLane.AddFragment(FragmentPlacement, NewTimeStart);
        }
    }
}
