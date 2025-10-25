using System;
using System.Threading.Tasks;
using Xabe.FFmpeg;
namespace VideoEditor.Core
{
    public class AddNewFragmentFromFileOperation : IOperation
    {
        public string Name => new string("AddNewFragment");
        private readonly FragmentPlacement FragmentPlacement;
        private readonly Lane Lane;
        public AddNewFragmentFromFileOperation(Project project, Fragment fragment, Lane lane)
        {
            //ToDo -- time duration
            FragmentPlacement = new FragmentPlacement(fragment);
            Lane = lane;
        }
        public static async Task<AddNewFragmentFromFileOperation> CreateAsync(Project project, string filePath, Lane lane)
        {
            TimeSpan duration = await GetVideoDuration(filePath);
            if (duration <= TimeSpan.FromSeconds(1))
            {
                //Default photo timespan
                duration=TimeSpan.FromSeconds(1);
            }
            Fragment fragment = new Fragment(filePath, duration);
            return new AddNewFragmentFromFileOperation(project, fragment, lane);
        }
        private static async Task<TimeSpan> GetVideoDuration(string filePath)
        {
            IMediaInfo info = await FFmpeg.GetMediaInfo(filePath);
            return info.Duration;
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