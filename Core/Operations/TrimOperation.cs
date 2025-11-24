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
            if (newStart > newEnd)
            {
                throw new ArgumentException($"Start {newStart} is greater than (or equal to) end {newEnd}");
            }
            FragmentPlacement = fragmentPlacement;
            var fragment = fragmentPlacement.Fragment;
            
            OldStart = fragment.StartTime;
            OldEnd = fragment.EndTime;

            NewStart = (newStart >= TimeSpan.Zero||fragment.FragmentType==Fragment.Type.Image) ? newStart:TimeSpan.Zero;//Image can be extended
            NewEnd = (newEnd<=fragment.FileDuration || fragment.FragmentType == Fragment.Type.Image) ? newEnd : fragment.FileDuration;//Image can be extended
        }
        public void Apply()
        {
            FragmentPlacement.Position += (NewStart - OldStart);

            FragmentPlacement.Fragment.StartTime = NewStart;
            FragmentPlacement.Fragment.EndTime = NewEnd;
        }
        public void Undo()
        {
            FragmentPlacement.Position -= (NewStart - OldStart);
            FragmentPlacement.Fragment.StartTime = OldEnd;
            FragmentPlacement.Fragment.EndTime = OldStart;
        }
    }
}