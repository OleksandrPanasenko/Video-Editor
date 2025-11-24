//Edit and render

//work with ffmpeg
using System;
using System.Diagnostics;
using System.Drawing;
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
            string tempDir = Path.Combine(_project.Path, "cache", "Render_" + Guid.NewGuid());
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
                string tempOut = Path.Combine(tempDir, $"part{index++}.mov");

                // Convert all inputs to video clips
                if (fragment.FragmentType == Fragment.Type.Image)
                {
                    await RunFFmpegAsync($"-framerate 1 -t {fragment.Duration.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture)} -i \"{input}\" -vf scale=1280:720,fps=30,format=yuva420p -y \"{tempOut}\"");
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
            await RunFFmpegAsync($"-f concat -safe 0 -i \"{listFile}\" -c:v libx264 -pix_fmt yuva420p -c:a aac -y \"{outputPath}\"");
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
            //Delete to ensure the new preview is displayed
            File.Delete(outputPath);
            
            //Clear if previous clear resulted in error
            string cache = Path.Combine(_project.Path, "cache");
            if (Directory.Exists(outputPath)) Directory.Delete(cache);
            Directory.CreateDirectory(cache);

            string tempDir = Path.Combine(cache, "Render_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);

            var sb = new StringBuilder();
            var lb = new StringBuilder();
            int index = 0;
            int laneIndex = 0;
            int indexGap = 0;

            double previousEnd = timestamp;

            
            foreach (var lane in ProjectContext.CurrentProject.Lanes)
            {
                previousEnd = timestamp;
                if(lane.Fragments.Count==0 || lane[TimeSpan.FromSeconds(timestamp),TimeSpan.FromSeconds(timestamp+durationSeconds)]==null) continue;
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
                    string tempOut = Path.Combine(tempDir, $"lane{laneIndex}part{index++}.mov");

                    string clipLengthStr = clipLength.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    // Build FFmpeg arguments
                    string args;
                    
                    if (fragment.FragmentType == Fragment.Type.Image)
                    {
                        args =
                            $"-loop 1 -t {clipLengthStr} -i \"{input}\" " + //loop image over period of time
                            //$"-f lavfi -t {clipLengthStr} -i anullsrc=channel_layout=stereo:sample_rate=48000 " +
                            // generate silent audio for concatenation
                            $"-f lavfi -i \"anullsrc=channel_layout=stereo:sample_rate=48000:duration={clipLengthStr},aformat=sample_fmts=s16:sample_rates=48000:channel_layouts=stereo\" " +
                            "-filter_complex \"[0:v]scale=640:360,fps=15[v]\" " +
                            $"-map \"[v]\" -map 1:a -c:v prores_ks -pix_fmt yuva444p10le -profile:v 4 -c:a pcm_s16le -ar 48000 -y \"{tempOut}\"";
                    }
                    else if (fragment.FragmentType == Fragment.Type.Audio)
                    {
                        string color = laneIndex == 0 ? "black" : "black@0.0";
                        args =
                            $"-f lavfi -i \"color=c={color}:s=640x360:d={clipLengthStr}\" " +
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +

                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" "+
                            "-map 0:v -map 1:a "+
                            $"-shortest -c:v prores_ks -pix_fmt yuva444p10le -profile:v 4 -c:a pcm_s16le -ar 48000 -y \"{tempOut}\"";
                    }
                    else // video
                    {
                        if (Path.GetExtension(fragment.FilePath) == ".gif")
                        {
                            args =
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +
                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" " +
                            $"-f lavfi -i \"anullsrc=channel_layout=stereo:sample_rate=48000:duration={clipLengthStr}\" " +
                            "-filter_complex \"[0:v]scale=640:360,fps=15[v]\" " +
                            "-map \"[v]\" -map 1:a -shortest " +
                            $"-c:v prores_ks -pix_fmt yuva444p10le -profile:v 4 -c:a pcm_s16le -ar 48000 -y \"{tempOut}\"";
                        }
                        else
                        {
                            args =
                            $"-ss {(fragment.StartTime.TotalSeconds + clipStart).ToString(CultureInfo.InvariantCulture)} " +
                            $"-t {clipLengthStr} " +
                            $"-i \"{input}\" -vf scale=640:360,fps=15 -c:v prores_ks -pix_fmt yuva444p10le -profile:v 4 -c:a pcm_s16le -ar 48000 -y \"{tempOut}\"";
                        }

                    }

                    //Make gap (empty video with silence)
                    if (previousEnd < placement.Position.TotalSeconds)
                    {
                        var gapLen = placement.Position.TotalSeconds - previousEnd;
                        string gapOut = Path.Combine(tempDir, $"gap{indexGap++}.mov");

                        // lane 0 = opaque, others = transparent
                        string color = laneIndex==0 ? "black" : "black@0.0";

                        
                        string durationStr = gapLen.ToString(CultureInfo.InvariantCulture);

                        string empty_view =
                            $"-f lavfi -i \"color=c={color}:s=640x360:d={durationStr}\" ";

                        string empty_sound =
                            $"-f lavfi -i \"anullsrc=channel_layout=stereo:sample_rate=48000:duration={durationStr}\" ";

                        await RunFFmpegAsync(
                            empty_view + empty_sound +
                            "-map 0:v -map 1:a " +
                            $"-shortest -c:v prores_ks -pix_fmt yuva444p10le -profile:v 4 -c:a pcm_s16le -ar 48000 -y \"{gapOut}\""
                        );

                        sb.AppendLine($"file '{gapOut.Replace("\\", "/")}'");
                    }

                    previousEnd = placement.EndPosition.TotalSeconds;
                    //Render fragment and add to list
                    await RunFFmpegAsync(args);
                    sb.AppendLine($"file '{tempOut.Replace("\\", "/")}'");

                }
                /****************************************************************************************************************************************/
                /*RENDER LANE*/
                /****************************************************************************************************************************************/
                string tempOutLane = Path.Combine(tempDir, $"lane{laneIndex++}.mov");
                string listFile = Path.Combine(tempDir, "list.txt");
                await File.WriteAllTextAsync(listFile, sb.ToString());
                
                await RunFFmpegAsync(
                    $"-f concat -safe 0 -i \"{listFile}\" -c copy -y \"{tempOutLane}\""
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
            string[] lanePaths = Directory.GetFiles(tempDir, "lane*.mov").Where(p => !Path.GetFileName(p).Contains("part")).ToArray();


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
                        filter.Append($"[0:v][1:v]overlay=x=0:y=0:format=auto:eof_action=pass[tmp1];");
                    else
                        filter.Append($"[tmp{i}][{i + 1}:v]overlay=x=0:y=0:format=auto:eof_action=pass[tmp{i + 1}];");
                }
                filter.Append($"[tmp{n - 1}]format=yuv420p[v];");
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
            // add all together to keep alpha channel
            string finalMovOut = Path.Combine(tempDir, "final_combined.mov");

            string movArgs =
                $"{inputs} -filter_complex \"{filter}\" -map \"[v]\" -map \"[a]\" -c:v prores_ks -pix_fmt yuv420p -profile:v 3 -c:a pcm_s16le -ar 48000 -y \"{finalMovOut}\"";

            await RunFFmpegAsync(movArgs);

            //Remove alpha
            string finalMp4Args =
                $"-i \"{finalMovOut}\" -t {durationSeconds.ToString(CultureInfo.InvariantCulture)} -c:v libx264 -pix_fmt yuv420p -c:a aac -ar 44100 -ac 2 -preset ultrafast -crf 28 -y \"{outputPath}\"";

            await RunFFmpegAsync(finalMp4Args);

            //try { Directory.Delete(tempDir, true); } catch(Exception ex)  { throw ex;/* ignore */ }
        }

        public async Task RenderOptimizedSCFGAsync(RenderParams args, string outputPath)
        {
            if (ProjectContext.CurrentProject.Lanes.Count == 0) return;

            // --- SETUP: Intermediate File and Parameters ---
            string tempDir = Path.Combine(_project.Path, "cache", "Render_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);
            string intermediateMovPath = Path.Combine(tempDir, "intermediate_output.mov");

            // Parse Resolution
            var res = args.Resolution.Split('x');
            int targetWidth = int.Parse(res[0]);
            int targetHeight = int.Parse(res[1]);

            // --- 1. BUILD SINGLE COMPLEX FILTER GRAPH (SCFG) ---
            var inputArgs = new StringBuilder();
            var filterComplex = new StringBuilder();
            var audioMixInputs = new StringBuilder();
            var inputMap = new Dictionary<string, int>();
            int inputIndex = 0;
            int laneCount = 0;

            foreach (var lane in _project.Lanes)
            {
                if (lane.Fragments.Count == 0) continue;

                var laneVideoInputs = new List<string>(); // Input tags for xfade/concat
                var laneAudioInputs = new List<string>();

                // Assume lane.Transitions is a list of objects associated with the transition time/type
                // For simplicity, we'll assume a basic fade transition for all.
                // In a real app, you'd calculate the exact start time of the transition based on placement.Position.
                double transitionDuration = 0.5; // Example fixed duration

                for (int i = 0; i < lane.Fragments.Count; i++)
                {
                    var placement = lane.Fragments[i];
                    var fragment = placement.Fragment;
                    string vTag = $"L{laneCount}C{i}v";
                    string aTag = $"L{laneCount}C{i}a";

                    // --- Input Mapping ---
                    if (!inputMap.ContainsKey(fragment.FilePath))
                    {
                        inputMap[fragment.FilePath] = inputIndex;
                        
                        inputArgs.Append($"-i \"{fragment.FilePath}\" ");
                        inputIndex++;
                    }
                    int fileIndex = inputMap[fragment.FilePath];

                    // --- Opacity and Hidden Logic (Video) ---
                    string finalOpacity = fragment.Hidden ? "0" : fragment.Opacity.ToString(CultureInfo.InvariantCulture);

                    // Trim, Scale, Opacity, and apply Text/Effects
                    // NOTE: Output is yuva444p for alpha channel support!
                    filterComplex.Append(
                        $"[{fileIndex}:v]trim=start={fragment.StartTime.TotalSeconds.ToString(CultureInfo.InvariantCulture)}:duration={fragment.Duration.TotalSeconds.ToString(CultureInfo.InvariantCulture)}," +
                        $"setpts=PTS-STARTPTS,scale={targetWidth}:{targetHeight}:force_original_aspect_ratio=decrease,pad={targetWidth}:{targetHeight}:(ow-iw)/2:(oh-ih)/2," +
                        $"format=yuva444p,colorchannelmixer=aa={finalOpacity}[{vTag}];"
                    );
                    laneVideoInputs.Add(vTag);

                    // --- Volume and Muted Logic (Audio) ---
                    string audioSourceFilter = "";
                    string audioInputTag = "";
                    bool usesAnullsrc = false;
                    string finalVolume = fragment.Muted ? "0" : fragment.Volume.ToString(CultureInfo.InvariantCulture);
                    string durationStr = fragment.Duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
                    // If the fragment is an Image or Text, we need to generate silent audio for its duration.
                    if (fragment.FragmentType == Fragment.Type.Image || fragment.FragmentType == Fragment.Type.Text||fragment.FilePath.EndsWith(".gif"))
                    {
                        // Generate silence within the filter_complex for the duration of the clip.
                        string anullSrcTag = $"anull{laneCount}c{i}";
                        filterComplex.AppendLine(
                            $"anullsrc=channel_layout=stereo:sample_rate=48000:duration={durationStr}[{anullSrcTag}];"
                        );
                        audioInputTag = anullSrcTag; // Source is the generated silence
                        usesAnullsrc = true;
                    }
                    else
                    {
                        // Audio comes from the input file (Video or Audio Fragment)
                        audioInputTag = $"{fileIndex}:a";
                    }

                    if (usesAnullsrc)
                    {
                        // Stream starts at 0, lasts for durationStr (already set in anullsrc).
                        filterComplex.Append(
                            $"[{audioInputTag}]asetpts=N/SR/TB,volume={finalVolume}[{aTag}];"
                        );
                    }
                    else
                    {
                        // Stream comes from a file, requiring trimming and time manipulation.
                        filterComplex.Append(
                            // We use the file's duration to avoid issues with fragmented streams
                            $"[{audioInputTag}]atrim=start={fragment.StartTime.TotalSeconds.ToString(CultureInfo.InvariantCulture)}:duration={durationStr}," +
                            $"asetpts=N/SR/TB,volume={finalVolume}[{aTag}];"
                        );
                    }


                   
                    laneAudioInputs.Add($"[{aTag}]");
                }

                // --- Stitch Lane Video Clips with XFADE ---
                string currentVTag = laneVideoInputs.First();
                for (int i = 1; i < laneVideoInputs.Count; i++)
                {
                    string nextVTag = laneVideoInputs[i];
                    string outputVTag = (i == laneVideoInputs.Count - 1) ? $"lane{laneCount}v" : $"tmp_v_{laneCount}_{i}";

                    // Calculate the exact offset for the xfade (time where the second clip starts relative to the timeline start).
                    // This is complex and relies on pre-calculating the final time of the combined clips *before* the transition.
                    // For a robust system, you'd calculate total duration, then subtract transition duration.
                    // Simplified approach using a placeholder:
                    double offset = (lane.Fragments[i - 1].Position.TotalSeconds + lane.Fragments[i - 1].Fragment.Duration.TotalSeconds) - transitionDuration;

                    // XFade: duration=T (how long the fade lasts), offset=O (when the fade starts in the new stream)
                    // Assumes a simple 'fade' transition type for now.
                    filterComplex.AppendLine(
                        $"[{currentVTag}][{nextVTag}]xfade=transition=fade:duration={transitionDuration.ToString(CultureInfo.InvariantCulture)}:offset={offset.ToString(CultureInfo.InvariantCulture)}[{outputVTag}];"
                    );
                    currentVTag = outputVTag;
                }

                // --- Concatenate Lane Audio Clips (AMIX is not used here) ---
                // We use concat for audio to avoid mixing issues when clips are sequential
                string concatATag = $"lane{laneCount}a";
                filterComplex.AppendLine($"{string.Join("", laneAudioInputs)}concat=n={laneAudioInputs.Count}:v=0:a=1[{concatATag}];");

                audioMixInputs.Append($"[{concatATag}]");
                laneCount++;
            }

            // --- Final Composition (Overlay and Mix) ---
            // 1. Video Overlay (Layering lanes)
            string finalVideoTag = "final_v";
            string currentOverlayVTag = "lane0v";

            for (int i = 1; i < laneCount; i++)
            {
                string nextVTag = $"lane{i}v";
                string outputVTag = (i == laneCount - 1) ? finalVideoTag : $"tmp_overlay{i}";
                // Overlay using the alpha channel built with yuva444p
                filterComplex.AppendLine($"[{currentOverlayVTag}][{nextVTag}]overlay=x=0:y=0:format=auto:eof_action=pass[{outputVTag}];");
                currentOverlayVTag = outputVTag;
            }
            if (laneCount == 1) finalVideoTag = "lane0v"; // If only one lane, use it directly

            // 2. Audio Mix (Combine all lane audio streams)
            string finalAudioTag = "final_a";
            if (laneCount > 1)
            {
                filterComplex.AppendLine($"{audioMixInputs}amix=inputs={laneCount}:normalize=0[final_a];");
            }
            else
            {
                filterComplex.AppendLine($"[lane0a]acopy[{finalAudioTag}];");
            }
            // --- PASS 1: SCFG to Intermediate MOV (main work) ---
            string pass1Args =
                $"{inputArgs.ToString().Trim()} -filter_complex \"{filterComplex.ToString().Trim()}\" " +
                // Map the final streams
                $"-map \"[{finalVideoTag}]\" -map \"[{finalAudioTag}]\" " +
                // Use a high-quality, alpha-supporting format
                $"-c:v prores_ks -pix_fmt yuva444p -c:a pcm_s16le -ar 48000 -y \"{intermediateMovPath}\"";

            await RunFFmpegAsync(pass1Args);

            // --- PASS 2: Convert MOV to Final MP4 (strips alpha, applies final compression) ---
            // Note: We use your RenderParams settings here.
            string pass2Args =
                $"-i \"{intermediateMovPath}\" -r {args.Fps} " +
                // We must manually add the pix_fmt yuv420p to strip the alpha channel
                $"-pix_fmt yuv420p " +
                $"{args.BuildArguments(outputPath)}";

            // Note: args.BuildArguments already includes -c:v libx264, etc.

            await RunFFmpegAsync(pass2Args);

            // Cleanup
            // try { Directory.Delete(tempDir, true); } catch { /* ignore */ }
        }


    }
}