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
            if(NewTimeStart<TimeSpan.Zero)
            {
                NewTimeStart = TimeSpan.Zero;
            }
        }
        public void Apply()
        {
            if (OldLane.HasTransition(FragmentPlacement)){
                throw new InvalidOperationException("Can't move fragment: Fragment has transitions.");
            }
            OldLane.RemoveFragment(FragmentPlacement);
            try
            {
                NewLane.AddFragment(FragmentPlacement, NewTimeStart);
            }
            catch (InvalidOperationException e)
            {
                //Revert
                OldLane.AddFragment(FragmentPlacement, OldTimeStart);
                throw new InvalidOperationException("Can't move fragment: "+e.Message);
            }
        }

        public void Undo()
        {
            NewLane.RemoveFragment(FragmentPlacement);
            OldLane.AddFragment(FragmentPlacement, OldTimeStart);
        }
    }
}
