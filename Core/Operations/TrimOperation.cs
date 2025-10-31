namespace VideoEditor.Core
{
    public class TrimOperation : IOperation
    {
        public string Name => new string("Trim");
        private readonly TimeSpan OldStart;
        private readonly TimeSpan OldEnd;
        private readonly TimeSpan NewStart;
        private readonly TimeSpan NewEnd;
        private readonly FragmentPlacement FragmentPlacement;
        public TrimOperation(Project project, FragmentPlacement fragmentPlacement, TimeSpan newStart, TimeSpan newEnd)
        {
            FragmentPlacement = fragmentPlacement;
            var fragment = fragmentPlacement.Fragment;
            
            OldStart = fragment.StartTime;
            OldEnd = fragment.EndTime;

            NewStart = newStart > TimeSpan.Zero||fragment.FragmentType==Fragment.Type.Image ? NewStart:TimeSpan.Zero;//Image can be extended
            NewEnd = newEnd<fragment.FileDuration || fragment.FragmentType == Fragment.Type.Image ? NewEnd : TimeSpan.Zero;//Image can be extended
        }
        public void Apply()
        {
            FragmentPlacement.Position += NewStart - OldStart;

            FragmentPlacement.Fragment.StartTime = NewStart;
            FragmentPlacement.Fragment.EndTime = NewEnd;
        }
        public void Undo()
        {
            FragmentPlacement.Fragment.StartTime = OldEnd;
            FragmentPlacement.Fragment.EndTime = OldStart;
        }
    }
}