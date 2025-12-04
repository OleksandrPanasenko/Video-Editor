using System.Drawing;
using Newtonsoft.Json;
using Videoeditor.Core;

namespace VideoEditor.Core
{
    //project metadata, files and lanes
    public class Project
    {
        public string Name { get; set; } // Project name
        public string Path { get; set; } // Project file path
        public ProjectConfig Configuration { get; set; } = new ProjectConfig(); // Project configuration settings
        public SelectionManager SelectionManager { get; set; } //Manages selected items in the project
        public List<Lane> Lanes { get; set; } = new List<Lane>(); // List of lanes in the project
        public List<string> MediaFiles { get; set; } = new List<string>(); // List of media file paths used in the project
        public History History; // Undo/Redo history manager
        [JsonIgnore]
        public Graphics Graphics { get; set; } // Graphics context for rendering
        public Engine engine = new Engine(); // Video processing engine
        public TimeSpan ProjectStart // Project start time across all lanes
        { 
            get 
            {
                TimeSpan? minStart = Lanes.Min(lane => lane.LaneStart);
                return minStart ?? TimeSpan.Zero;
            } 
        }
        public TimeSpan ProjectEnd // Project end time across all lanes
        {
            get
            {
                var End = TimeSpan.Zero;
                foreach (var lane in Lanes)
                {
                    if (lane.LaneEnd != null & lane.LaneEnd > End)
                    {
                        End = (TimeSpan)lane.LaneEnd;
                    }
                }
                return End;
            }
        }
        public TimeSpan ProjectDuration { get { return ProjectEnd - ProjectStart; } }
        public void AddMediaFile(string filePath)
        {
            if (!MediaFiles.Contains(filePath))
            {
                MediaFiles.Add(filePath);
            }
        }
        public void RemoveMediaFile(string filePath)
        {
            if (MediaFiles.Contains(filePath))
            {
                MediaFiles.Remove(filePath);
            }
        }
        public void AddLane()
        {
            int laneNumber = 1;
            while(Lanes.Exists(l => l.Name == $"Lane {laneNumber}"))
            {
                laneNumber++;
            }
            var lane = new Lane($"Lane {laneNumber}");
            Lanes.Add(lane);
        }
        public void RemoveLane(int laneNumber)
        {
            Lanes.RemoveAt(laneNumber);
        }
        //Render lane panel
        public void RenderPanel()
        {
            var renderer = new RenderLanePanel(this, Graphics);
            renderer.Render();
        }

        

        public Project()
        {
            History = new History(Config.MaxUndoSteps);
            SelectionManager = new SelectionManager(this);
            AddLane(); // Start with one lane by default
            Name = "Untitled Project";
        }
    }
}