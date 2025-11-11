using VideoEditor.Core.Effects;
using VideoEditor.Core.Transitions;

namespace VideoEditor.Core
{
    public class Fragment
    {
        public string FilePath { get; set; }
        public TimeSpan FileDuration { get; set; }
        public string FileName { get; set; }
        public bool Subtitles {  get; set; }=false;
        public string Name
        {
            get => string.IsNullOrEmpty(FileName) ? System.IO.Path.GetFileName(FilePath) : FileName; 
            set => FileName = value; 
        }
        public TimeSpan StartTime { get; set; } // Start time in milliseconds
        public TimeSpan EndTime { get; set; } // End time in milliseconds
        public TimeSpan Duration => EndTime - StartTime; // Duration in milliseconds

        public float Volume = 1;
        public float Opacity = 1;
        public bool Muted { get; set; }
        public bool Hidden { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<Effect> Effects { get; set; } = new();
        public Transition? OutTransition { get; set; }
        public Fragment(string filePath, TimeSpan startTime, TimeSpan endTime)
        {
            FilePath = filePath;
            StartTime = startTime;
            EndTime = endTime;
        }
        public Fragment(string filePath, TimeSpan duration)
        {
            FilePath = filePath;
            StartTime = TimeSpan.Zero;
            EndTime = duration;
            FileDuration= duration;
        }
        public Fragment DeepCopy()
        {
            Fragment newFragment= new Fragment(FilePath,StartTime,EndTime);
            newFragment.Hidden = Hidden;
            newFragment.Muted = Muted;
            newFragment.Volume = Volume;
            newFragment.Width = Width;
            newFragment.Height = Height;
            newFragment.Opacity = Opacity;
            newFragment.FileDuration= FileDuration;

            return newFragment;
        }
        
        public enum Type {Video, Audio, Image, Text, Unknown};
        public static Type FileType(string path)
        {
            string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();

            if (new[] { ".mp4", ".avi", ".mkv", ".mov", ".gif" }.Contains(ext))
                return Type.Video;
            if (new[] { ".mp3", ".wav", ".flac" }.Contains(ext))
                return Type.Audio;
            if (new[] { ".jpg", ".jpeg", ".png", ".bmp"}.Contains(ext))
                return Type.Image;
            if (new[] {".txt", "srt",".ass"}.Contains(ext))
                return Type.Text;
            return Type.Unknown;
        }
        public virtual Type FragmentType { get { return FileType(FilePath); } }
        //Empty constructor for deserialisation
        public Fragment() { }
    }
}