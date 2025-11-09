//Edit and render

//work with ffmpeg
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
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
            var lb = new StringBuilder();
            int index = 0;
            int laneIndex = 0;
            int indexGap = 0;

            double previousEnd = timestamp;

            // We assume one lane for MVP
            foreach (var lane in ProjectContext.CurrentProject.Lanes)
            {
                foreach (var placement in lane.Fragments)
                {
                    var fragment = placement.Fragment;
                    double start = placement.Position.TotalSeconds;
                    double end = placement.EndPosition.TotalSeconds;


                    // Include only if it overlaps the preview window
                    if (end < timestamp || start > timestamp + durationSeconds)
                        continue;

                    double clipStart = Math.Max(0, timestamp - start);
                    double clipLength = Math.Min(fragment.Duration.TotalSeconds - clipStart, Math.Min(durationSeconds, timestamp - start + durationSeconds));

                    string input = fragment.FilePath;
                    string tempOut = Path.Combine(tempDir, $"lane{laneIndex}part{index++}.mp4");

                    string clipLengthStr = clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    // Build FFmpeg arguments
                    string args;
                    if (fragment.FragmentType == Fragment.Type.Image)
                    {
                        args =
                            $"-loop 1 -t {clipLengthStr} -i \"{input}\" " +
                            //$"-f lavfi -t {clipLengthStr} -i anullsrc=channel_layout=stereo:sample_rate=44100 " +
                            $"-f lavfi -i \"anullsrc=channel_layout=stereo:sample_rate=44100:duration={clipLengthStr},aformat=sample_fmts=s16:sample_rates=44100:channel_layouts=stereo\" " +
                            "-filter_complex \"[0:v]scale=640:360,format=yuv420p[v]\" " +
                            "-map \"[v]\" -map 1:a -shortest -c:v libx264 -c:a aac -ar 44100 -ac 2 -b:a 128k -shortest -preset ultrafast -y \"" + tempOut + "\"";
                    }
                    else if (fragment.FragmentType == Fragment.Type.Audio)
                    {
                        args =
                            $"-f lavfi -i \"color=c=black:s=640x360:d={clipLengthStr}\" " +
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +

                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" -shortest -c:v libx264 -c:a aac -ar 44100 -ac 2 -preset ultrafast -y \"{tempOut}\"";
                    }
                    else // video
                    {
                        if (Path.GetExtension(fragment.FilePath) == ".gif")
                        {
                            args =
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +
                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" " +
                            $"-f lavfi -i \"anullsrc=channel_layout=stereo:sample_rate=44100:duration={clipLengthStr},aformat=sample_fmts=s16:sample_rates=44100:channel_layouts=stereo\" " +
                            "-filter_complex \"[0:v]scale=640:360,fps=15,format=yuv420p[v]\" " +
                            "-map \"[v]\" -map 1:a -shortest " +
                            $"-c:v libx264 -c:a aac -ar 44100 -ac 2 -preset ultrafast -y \"{tempOut}\"";
                        }
                        else
                        {
                            args =
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +
                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" -vf scale=640:360,fps=15 -c:v libx264 -c:a aac -ar 44100 -ac 2 -preset ultrafast -y \"{tempOut}\"";
                        }

                    }

                    //Make gap (empty video with silence)
                    if (previousEnd < placement.Position.TotalSeconds)
                    {
                        var gapLen = placement.Position.TotalSeconds - previousEnd;
                        string gapOut = Path.Combine(tempDir, $"gap{indexGap++}.mp4");
                        string color = /*isTopLane ? "black@0.0" : */"black";
                        await RunFFmpegAsync($"-f lavfi -t {gapLen.ToString(CultureInfo.InvariantCulture)} -i \"color=c={color}:s=640x360\" -pix_fmt {/*(isTopLane ? "yuva420p" :*/ ("yuv420p")} -preset ultrafast -y \"{gapOut}\"");
                        sb.AppendLine($"file '{gapOut.Replace("\\", "/")}'");
                    }

                    previousEnd = placement.EndPosition.TotalSeconds;
                    //Render fragment and add to list
                    await RunFFmpegAsync(args);
                    sb.AppendLine($"file '{tempOut.Replace("\\", "/")}'");

                }
                string tempOutLane = Path.Combine(tempDir, $"lane{laneIndex++}.mp4");
                string listFile = Path.Combine(tempDir, "list.txt");
                await File.WriteAllTextAsync(listFile, sb.ToString());
                
                await RunFFmpegAsync(
                    $"-f concat -safe 0 -i \"{listFile}\" -c:v libx264 -pix_fmt yuv420p -c:a aac -preset ultrafast -y \"{tempOutLane}\""
                );
                lb.AppendLine($"file '{tempOutLane.Replace("\\", "/")}'");

                //render lane and add to list
                sb=new StringBuilder();

                index = 0;
            }
            // Concatenate partials
            string listLaneFile = Path.Combine(tempDir, "listLane.txt");
            await File.WriteAllTextAsync(listLaneFile, lb.ToString());
            //add all lines in files together
            string[] lanePaths = Directory.GetFiles(tempDir, "lane*.mp4");


            var inputs = string.Join(" ", lanePaths.Select(p => $"-i \"{p}\""));
            int n = lanePaths.Length;

            // build overlay chain
            var filter = new StringBuilder();

            // video overlay chain
            if (n > 1)
            {
                for (int i = 0; i < n - 1; i++)
                {
                    if (i == 0)
                        filter.Append($"[0:v][1:v]overlay=shortest=1[tmp1];");
                    else
                        filter.Append($"[tmp{i}][{i + 1}:v]overlay=shortest=1[tmp{i + 1}];");
                }
                filter.Append($"[tmp{n - 1}]copy[v];");
            }
            else
            {
                filter.Append("[0:v]copy[v];");
            }
            // audio mix
            filter.Append($"[0:a]");
            for (int i = 1; i < n; i++)
                filter.Append($"[{i}:a]");
            filter.Append($"amix=inputs={n}:normalize=0[a]");
            // add all together
            string finalArgs =
                $"{inputs} -filter_complex \"{filter}\" -map \"[v]\" -map \"[a]\" -c:v libx264 -c:a aac -ar 44100 -ac 2 -preset ultrafast -y \"{outputPath}\"";

            await RunFFmpegAsync(finalArgs);

            try { Directory.Delete(tempDir, true); } catch { /* ignore */ }
        }



    }
}