using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Operations
{
    public class RenderParams
    {
        public string Resolution { get; set; }
        public int Fps { get; set; }
        public string VideoFormat { get; set; }
        public string AudioFormat { get; set; }
        public string AudioBitrate { get; set; }
        public RenderParams() { }

        public string BuildArguments(string outputPath)
        {
            // приклад для MP4 з AAC
            var args = $"-r {Fps} -s {Resolution} -b:a {AudioBitrate} ";

            if (VideoFormat == "mp4") args += "-c:v libx264 -pix_fmt yuv420p ";
            if (VideoFormat == "avi") args += "-c:v libxvid ";
            if (VideoFormat == "mov") args += "-c:v prores_ks ";
            if (VideoFormat == "mkv") args += "-c:v libx264 ";

            if (AudioFormat == "aac") args += "-c:a aac ";
            if (AudioFormat == "mp3") args += "-c:a libmp3lame ";
            if (AudioFormat == "wav") args += "-c:a pcm_s16le ";
            if (AudioFormat == "flac") args += "-c:a flac ";

            args += $"\"{outputPath}\"";
            return args;
        }
    }
}
