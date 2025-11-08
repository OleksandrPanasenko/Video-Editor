//Edit and render

//work with ffmpeg
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;
using VideoEditor.Core.Operations;
using Xabe.FFmpeg;

namespace Videoeditor.Core { 
    public class Engine
    {

        Project _project { get => ProjectContext.CurrentProject; }
        public void EditVideo(string inputPath, string outputPath)
        {
            
        }
        //Render video
        public async Task RenderAsync(RenderParams args, string outputPath)
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
                    await RunFFmpegAsync($"-framerate 1 -t {fragment.Duration.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture)} -i \"{input}\" -vf scale=1280:720,fps=30,format=yuv420p -y \"{tempOut}\"");
                }
                else if (fragment.FragmentType == Fragment.Type.Audio)
                {
                    await RunFFmpegAsync($"-f lavfi -i \"color=c=black:s=1280x720:d={fragment.Duration.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" -i \"{input}\" -shortest -c:v libx264 -c:a aac -y \"{tempOut}\"");
                }
                else // video
                {
                    await RunFFmpegAsync($"-i \"{input}\" -vf scale=1280:720,fps=30 -c:v libx264 -c:a aac -y \"{tempOut}\"");
                }

                sb.AppendLine($"file '{tempOut.Replace("\\", "/")}'");
            }

            await File.WriteAllTextAsync(listFile, sb.ToString());

            // Concatenate all parts
            await RunFFmpegAsync($"-f concat -safe 0 -i \"{listFile}\" -c:v libx264 -pix_fmt yuv420p -c:a aac -y \"{outputPath}\"");
        }

        private static async Task RunFFmpegAsync(string arguments)
        {
            var ffmpegPath = @"C:\ffmpeg\ffmpeg.exe";
            var psi = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var proc = new Process{StartInfo= psi };
            proc.Start();

            var stdOutTask = Task.Run(() => proc.StandardOutput.ReadToEndAsync());
            var stdErrTask = Task.Run(() => proc.StandardError.ReadToEndAsync());

            await proc.WaitForExitAsync();

            string stdOut = await stdOutTask;
            string stdErr = await stdErrTask;

            if (proc.ExitCode != 0)
                throw new Exception($"FFmpeg failed with code {proc.ExitCode}: {stdErr}");
        }

        public void RenderVideo(string projectFilePath, string outputPath)
        {
            // Example: Render a video project (this is a placeholder, actual implementation may vary)
            // Load project file and render using FFmpeg or other libraries
            /*var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(projectFilePath, null, outputPath, null);*/

        }

        public async Task RenderPreviewAsync(string outputPath, double timestamp, double durationSeconds)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "Preview_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);

            var sb = new StringBuilder();
            int index = 0;
            int indexGap = 0;

            double previousEnd = timestamp;

            // We assume one lane for MVP
            foreach (var placement in ProjectContext.CurrentProject.Lanes[0].Fragments)
            {
                var fragment = placement.Fragment;
                double start = placement.Position.TotalSeconds;
                double end = placement.EndPosition.TotalSeconds;

                
                // Include only if it overlaps the preview window
                if (end < timestamp || start > timestamp + durationSeconds)
                    continue;

                double clipStart = Math.Max(0, timestamp - start);
                double clipLength = Math.Min(fragment.Duration.TotalSeconds - clipStart, Math.Min(durationSeconds, timestamp-start+durationSeconds));
                
                string input = fragment.FilePath;
                string tempOut = Path.Combine(tempDir, $"part{index++}.mp4");



                // Build FFmpeg arguments
                string args;
                if (fragment.FragmentType == Fragment.Type.Image)
                {
                    args =
                        $"-loop 1 -t {clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                        $"-i \"{input}\" -vf scale=640:360,fps=15,format=yuv420p -preset ultrafast -y \"{tempOut}\"";
                }
                else if (fragment.FragmentType == Fragment.Type.Audio)
                {
                    args =
                        $"-f lavfi -i \"color=c=black:s=640x360:d={clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" " +
                        $"-ss {(fragment.StartTime.TotalSeconds+clipStart).ToString(System.Globalization.CultureInfo.InvariantCulture)} " + 
                        
                        $"-t {clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                        $"-i \"{input}\" -shortest -c:v libx264 -c:a aac -preset ultrafast -y \"{tempOut}\"";
                }
                else // video
                {
                    args =
                        $"-ss {(fragment.StartTime.TotalSeconds+clipStart).ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                        $"-t {clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture)} " +
                        $"-i \"{input}\" -vf scale=640:360,fps=15 -c:v libx264 -c:a aac -preset ultrafast -y \"{tempOut}\"";
                }

                //Make gap
                if(previousEnd<placement.Position.TotalSeconds)
                {
                    var gapLen = placement.Position.TotalSeconds - previousEnd;
                    string gapOut = Path.Combine(tempDir, $"gap{indexGap++}.mp4");
                    string color = /*isTopLane ? "black@0.0" : */"black";
                    await RunFFmpegAsync($"-f lavfi -t {gapLen.ToString(CultureInfo.InvariantCulture)} -i \"color=c={color}:s=640x360\" -pix_fmt {/*(isTopLane ? "yuva420p" :*/ ("yuv420p")} -preset ultrafast -y \"{gapOut}\"");
                    sb.AppendLine($"file '{gapOut.Replace("\\", "/")}'");
                }

                previousEnd = placement.EndPosition.TotalSeconds;

                await RunFFmpegAsync(args);
                sb.AppendLine($"file '{tempOut.Replace("\\", "/")}'");
            }

            // Concatenate partials
            string listFile = Path.Combine(tempDir, "list.txt");
            await File.WriteAllTextAsync(listFile, sb.ToString());

            await RunFFmpegAsync(
                $"-f concat -safe 0 -i \"{listFile}\" -c:v libx264 -pix_fmt yuv420p -c:a aac -preset ultrafast -y \"{outputPath}\""
            );

            try { Directory.Delete(tempDir, true); } catch { /* ignore */ }
        }



    }
}