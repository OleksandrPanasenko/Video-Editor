//Edit and render

//work with ffmpeg
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;
using Xabe.FFmpeg;

namespace Videoeditor.Core { 
    public class Engine
    {
        Project _project { get => ProjectContext.CurrentProject; }
        public void EditVideo(string inputPath, string outputPath)
        {
            // Example: Use FFmpeg to trim a video
            /*var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(inputPath, null, outputPath, null, new NReco.VideoConverter.ConvertSettings()
            {
                CustomOutputArgs = "-ss 00:00:10 -t 00:00:20" // Trim from 10s to 30s
            });*/
        }
        //Render video ito mp4 format
        public async Task RenderAsync(string outputPath)
        {
            //Create temporary directory for render
            string tempDir = Path.Combine(Path.GetTempPath(), "Render_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);

            //File to store addresses of temporary videos
            var listFile = Path.Combine(tempDir, "list.txt");
            var sb = new StringBuilder();

            //Iterate through all lanes and fragments
            int index = 0;
            foreach (var fragmentPlacement in ProjectContext.CurrentProject.Lanes[0].Fragments)
            {
                var fragment=fragmentPlacement.Fragment;
                string input = fragment.FilePath;
                string tempOut = Path.Combine(tempDir, $"part{index++}.mp4");

                // Convert all inputs to video clips
                if (fragment.FragmentType == Fragment.Type.Image)
                {
                    await RunFFmpegAsync($"-loop 1 -t {fragment.Duration.TotalSeconds} -i \"{input}\" -vf scale=1280:720,fps=30 -y \"{tempOut}\"");
                }
                else if (fragment.FragmentType == Fragment.Type.Audio)
                {
                    await RunFFmpegAsync($"-loop 1 -f lavfi -i color=c=black:s=1280x720:d={fragment.Duration.TotalSeconds} -i \"{input}\" -shortest -c:v libx264 -c:a aac -y \"{tempOut}\"");
                }
                else // video
                {
                    await RunFFmpegAsync($"-i \"{input}\" -vf scale=1280:720,fps=30 -c:v libx264 -c:a aac -y \"{tempOut}\"");
                }

                sb.AppendLine($"file '{tempOut.Replace("\\", "/")}'");
            }

            await File.WriteAllTextAsync(listFile, sb.ToString());

            // Concatenate all parts
            await RunFFmpegAsync($"-f concat -safe 0 -i \"{listFile}\" -c copy -y \"{outputPath}\"");
        }

        private static async Task RunFFmpegAsync(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            await proc.WaitForExitAsync();
        }

        public void RenderVideo(string projectFilePath, string outputPath)
        {
            // Example: Render a video project (this is a placeholder, actual implementation may vary)
            // Load project file and render using FFmpeg or other libraries
            /*var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(projectFilePath, null, outputPath, null);*/

        }
    }
}