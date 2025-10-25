namespace VideoEditor.Core
{
    public class Fragment
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Name
        {
            get => string.IsNullOrEmpty(FileName) ? System.IO.Path.GetFileName(FilePath) : FileName; 
            set => FileName = value; 
        }
        public TimeSpan StartTime { get; set; } // Start time in milliseconds
        public TimeSpan EndTime { get; set; } // End time in milliseconds
        public TimeSpan Duration => EndTime - StartTime; // Duration in milliseconds
        public double Width { get; set; }
        public double Height { get; set; }
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
        }
        public Fragment CopyFragment()
        {
            return new Fragment(FilePath, StartTime, EndTime);
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
        public Type FragmentType { get { return FileType(FilePath); } }
    }
}