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
        public AddNewFragmentFromFileOperation(Project project, string FilePath, Lane lane)
        {
            
            TimeSpan duration = GetVideoDuration(FilePath).GetAwaiter().GetResult();

            Fragment fragment = new Fragment(FilePath,duration);//ToDo -- time duration
            FragmentPlacement = new FragmentPlacement(fragment);
            Lane = lane;
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