using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class CopyOperation:IOperation
    {
        FragmentPlacement NewCopy;
        FragmentPlacement? OldCopy;
        Project Project;
        public CopyOperation(Project project, FragmentPlacement fragmentPlacement) {
            Project = project;
            if (Project != null&& fragmentPlacement!=null) {
                OldCopy = Project.SelectionManager.MemoryFragment;
                NewCopy = fragmentPlacement.DeepCopy();
            }
        }

        public string Name => "Copy";

        public void Apply()
        {
            Project.SelectionManager.MemoryFragment=NewCopy;
        }

        public void Undo()
        {
            Project.SelectionManager.MemoryFragment=OldCopy;
        }
    }
}
