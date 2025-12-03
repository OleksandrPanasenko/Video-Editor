using System.Drawing;
using Newtonsoft.Json;
using Videoeditor.Core;

namespace VideoEditor.Core
{
    //project metadata, files and lanes
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectConfig Configuration { get; set; } = new ProjectConfig();
        public SelectionManager SelectionManager { get; set; }
        public List<Lane> Lanes { get; set; } = new List<Lane>();
        public List<string> MediaFiles { get; set; } = new List<string>();
        public History History;
        [JsonIgnore]
        public Graphics Graphics { get; set; }
        public Engine engine = new Engine();
        public TimeSpan ProjectStart{ get {
                var Start=TimeSpan.Zero;
                foreach (var lane in Lanes)
                {
                    if (lane.LaneStart!=null & lane.LaneStart > Start)
                    {
                        Start = (TimeSpan)lane.LaneStart;
                    }
                }
                return Start;
            } }
        public TimeSpan ProjectEnd
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
        //Render 
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