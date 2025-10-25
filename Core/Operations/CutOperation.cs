namespace VideoEditor.Core
{
    public class CutOperation:IOperation
    {
        public string Name => new string("Cut");
        private readonly FragmentPlacement First;
        private readonly FragmentPlacement Second;
        private readonly Project Project;
        private readonly Lane Lane;
        private TimeSpan CutOffset;

        public CutOperation(Project project, Lane lane, FragmentPlacement placement, TimeSpan cutOffset)
        {
            Project = project;
            Lane = lane;
            First = placement;
            Second = new FragmentPlacement(placement.Fragment.CopyFragment());
        }
        public void Apply()
        {
            new TrimOperation(Project, First.Fragment, First.Fragment.StartTime, CutOffset).Apply();
            new TrimOperation(Project, Second.Fragment, CutOffset, Second.Fragment.EndTime).Apply();
            Lane.AddFragment(Second, First.EndPosition);
        }
        public void Undo()
        {
            Lane.RemoveFragment(Second);
            new TrimOperation(Project, First.Fragment, First.Fragment.StartTime, Second.Fragment.EndTime).Apply();
        }
    }
}