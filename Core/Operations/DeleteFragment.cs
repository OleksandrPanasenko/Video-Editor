using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;

namespace Core.Operations
{
    public class DeleteFragmentOperation:IOperation
    {
        public string Name => new string("Delete");
        FragmentPlacement FragmentPlacement;
        Lane Lane;
        public DeleteFragmentOperation(FragmentPlacement fragmentPlacement, Lane lane) { 
            FragmentPlacement = fragmentPlacement;
            Lane = lane;
        }
        public void Apply()
        {
            Lane.RemoveFragment(FragmentPlacement);
        }
        public void Undo()
        {
            Lane.AddFragment(FragmentPlacement,FragmentPlacement.Position);
        }
    }
}
