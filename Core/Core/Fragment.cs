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
        public enum Type {Video, Audio, Photo, Text};
        public Type FragmentType { get; set; } = Type.Video;
    }
}