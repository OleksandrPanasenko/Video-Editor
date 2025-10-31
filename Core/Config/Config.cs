using System.Drawing;


namespace VideoEditor.Core
{
    public static class Config
    {
        public static string FfmpegPath { get; set; } = "ffmpeg"; // Default to ffmpeg in PATH
        public static string FfprobePath { get; set; } = "ffprobe"; // Default to ffprobe in PATH
        public static string TempDirectory { get; set; } = Path.GetTempPath();
        public static int MaxUndoSteps { get; set; } = 20;
        public static string DefaultProjectExtension { get; set; } = ".vep";
        public static string DefaultExportFormat { get; set; } = "mp4";
        public static int AutoSaveIntervalMinutes { get; set; } = 5;
        //public static bool EnableHardwareAcceleration { get; set; } = false;
        //public static string HardwareAccelerationMethod { get; set; } = "auto"; // e.g., "cuda", "dxva2", "qsv", etc.
        public static Color LanePanelBackgroundColor = Color.FromArgb(43, 43, 43);
        public static Color LaneBackgroundColor= Color.FromArgb(58, 58, 58);
        public static Color LaneBorderColor = Color.FromArgb(80, 80, 80);

        public static Color LaneSelectedBackgroundColor = Color.FromArgb((int)(LaneBackgroundColor.R * 0.6), (int)(LaneBackgroundColor.G * 0.6), (int)(LaneBackgroundColor.B * 0.6));


        public static Color VideoBlockColor = Color.FromArgb(46, 134, 193);
        public static Color AudioBlockColor = Color.FromArgb(39, 174, 96);
        public static Color TextBlockColor = Color.FromArgb(211, 84, 0);
        public static Color ImageBlockColor = Color.FromArgb(155, 89, 182);

        public static Color SelectionHighlightColor = Color.FromArgb(241, 196, 15);


        public static Color TimePointerColor = Color.FromArgb(231, 76, 60);
        public static Color ButtonBackgroundColor = Color.FromArgb(68, 68, 68);
        public static Color ButtonHoverColor = Color.FromArgb(102, 102, 102);
        public static Color ButtonPressedColor = Color.FromArgb(136, 136, 136);
        public static Color TimeRulerColor = Color.Red;
        public static Font FragmentNameFont= new Font("Segoe UI", 9);

        public static float TimeRulerMinDistancePixels = 60; // Minimum distance between time markers in pixels

        public static float TimeZoomRatio = (float)1.1;
        public static float LaneZoomRatio = (float)1.1;
    }
}