using VideoEditor.Core.Effects;
using VideoEditor.Core.Transitions;

namespace VideoEditor.Core
{
    public class Fragment
    {
        public string FilePath { get; set; } // Path to the media file
        public TimeSpan FileDuration { get; set; } // Duration of the original file
        public string FileName { get; set; } 
        public bool Subtitles {  get; set; }=false;// Whether to show subtitles (for text fragments)
        public string Name// Display name of the fragment
        {
            get => string.IsNullOrEmpty(FileName) ? System.IO.Path.GetFileName(FilePath) : FileName; 
            set => FileName = value; 
        }
        public TimeSpan StartTime { get; set; } // Start time in milliseconds
        public TimeSpan EndTime { get; set; } // End time in milliseconds
        public TimeSpan Duration => EndTime - StartTime; // Duration in milliseconds

        public float Volume = 1; // Volume level (0.0 to 1.0)
        public float Opacity = 1; // Opacity level (0.0 to 1.0)
        public bool Muted { get; set; }
        public bool Hidden { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<IEffect> Effects { get; set; } = new();
        public ITransition? OutTransition { get; set; }
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
        // Create a deep copy of the fragment
        public virtual Fragment DeepCopy()
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
        //Get file type based on extension
        public static Type FileType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Type.Unknown;
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