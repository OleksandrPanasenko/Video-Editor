namespace VideoEditor.Core
{
    public class TrimOperation : IOperation
    {
        public string Name => new string("Trim");
        private readonly TimeSpan OldStart;
        private readonly TimeSpan OldEnd;
        private readonly TimeSpan NewStart;
        private readonly TimeSpan NewEnd;
        private readonly Fragment Fragment;
        public TrimOperation(Project project, Fragment fragment, TimeSpan newStart, TimeSpan newEnd)
        {
            Fragment = fragment;
            OldStart = fragment.StartTime;
            OldEnd = fragment.EndTime;
            NewStart = newStart;
            NewEnd = newEnd;
        }
        public void Apply()
        {
            Fragment.StartTime = NewStart;
            Fragment.EndTime = NewEnd;
        }
        public void Undo()
        {
            Fragment.StartTime = OldEnd;
            Fragment.EndTime = OldStart;
        }
    }
}