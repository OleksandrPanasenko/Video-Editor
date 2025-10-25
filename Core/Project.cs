using System.Drawing;

namespace VideoEditor.Core
{
    //project metadata, files and lanes
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        internal ProjectConfig Configuration { get; set; } = new ProjectConfig();
        public SelectionManager SelectionManager { get; set; }
        public List<Lane> Lanes { get; set; } = new List<Lane>();
        public List<string> MediaFiles { get; set; } = new List<string>();
        public History History;
        public Graphics Graphics { get; set; }
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