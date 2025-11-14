using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class AddTextFragmentOperation : IOperation
    {
        public string Name => "Add text fragment";
        FragmentPlacement Placement;
        TimeSpan Time;
        Lane Lane;

        public AddTextFragmentOperation(TextFragment fragment,TimeSpan time,Lane lane)
        {
            Lane= lane;
            Placement=new FragmentPlacement(fragment);
            Time=time;
            fragment.EndTime = TimeSpan.FromSeconds(1);
        }
        public void Apply()
        {
            Lane.AddFragment(Placement, Time); ;
        }

        public void Undo()
        {
            Lane.RemoveFragment(Placement);
        }
    }
}
