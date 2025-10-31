using Core.Operations;
using VideoEditor.Core.Operations;

namespace VideoEditor.Core
{
    public class CutOperation:IOperation
    {
        public string Name => "Cut";
        Project Project;
        Lane Lane
        FragmentPlacement FragmentPlacement;
        public CutOperation(Project project, Lane lane, FragmentPlacement fragmentPlacement)
        {

        }
        public void Apply()
        {
            new CopyOperation(Project,FragmentPlacement).Apply();
            new DeleteFragmentOperation(FragmentPlacement, Lane).Apply();
        }

        public void Undo()
        {
            //TODO Paste
        }
    }
}