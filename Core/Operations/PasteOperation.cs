using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class PasteOperation : IOperation
    {
        FragmentPlacement FragmentPlacement;
        Lane Lane;
        TimeSpan InsertTime;
        public string Name => "Paste";
        public PasteOperation(FragmentPlacement fragmentPlacement, Lane lane, TimeSpan insertTime)
        {
            FragmentPlacement = fragmentPlacement.DeepCopy();
            Lane = lane;
            InsertTime = insertTime;
        }

        public void Apply()
        {
            Lane.AddFragment(FragmentPlacement,InsertTime);
        }

        public void Undo()
        {
            Lane.RemoveFragment(FragmentPlacement);
        }
    }
}
