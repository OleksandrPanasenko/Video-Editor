using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public class SelectionManager
    {
        [JsonIgnore]
        public Project? Project { get; set; }
        public FragmentPlacement? SelectedFragment { get; set; }
        public TimeSpan? SelectedTime { get; set; }
        public Lane? SelectedLane { get; set; }
        public String? SelectedMedia { get; set; }
    
        //public bool IsDragging { get; set; }
        public ProjectConfig? Params { get { return Project.Configuration; }}
        public SelectionManager(Project project)
        {
            Project = project;
        }
        public void ClearSelection()
        {
            SelectedFragment = null;
            SelectedTime = null;
            SelectedLane = null;
        }
        public void SelectObject(int x, int y)
        {
            if (y < 0 || x < 0 || y > Params.LanePanelHeight || x > Params.LanePanelWidth)
            {
                ClearSelection();
            }
            else
            {
                int laneIndex = (int)((y + Params.LanePanelScrollY) / (Params.LaneHeight + Params.LaneSpacing));
                if (laneIndex >= Project.Lanes.Count)
                {
                    ClearSelection();
                    return;
                }
                SelectedLane = Project.Lanes[laneIndex];
                TimeSpan time = TimeSpan.FromSeconds((x + Params.LanePanelScrollX-Params.LaneLabelWidth) / Params.LaneTimeScale);// Maybe make ScrollX in seconds?
                SelectedTime = time;
                SelectedFragment = SelectedLane[time];
            }
        }
        public void SelectObject(Point point)
        {
            SelectObject(point.X, point.Y);
        }
    }
}
