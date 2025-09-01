using FFl
namespace VideoEditor.Core
{
    public class AddNewFragmentFromFileOperation : IOperation
    {
        public string Name => new string("AddNewFragment");
        private readonly FragmentPlacement FragmentPlacement;
        private readonly Lane Lane;
        public AddNewFragmentFromFileOperation(Project project, string FilePath)
        {
            Fragment fragment = new Fragment(FilePath,);//ToDo -- time duration
            FragmentPlacement = new FragmentPlacement(fragment);
        }
        public void Apply()
        {
            Lane.AddFragment(FragmentPlacement);
        }
        public void Undo()
        {
            Lane.RemoveFragment(FragmentPlacement);
        }
    }
}