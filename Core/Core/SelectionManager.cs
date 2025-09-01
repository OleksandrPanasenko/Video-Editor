using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public class SelectionManager
    {
        public Project Project { get; set; }
        public FragmentPlacement? SelectedFragment { get; set; }
        public TimeSpan? SelectedTime { get; set; }
        public Lane? SelectedLane { get; set; }
        public 
        //public bool IsDragging { get; set; }
        internal ProjectConfig Params;
        public SelectionManager(Project project)
        {
            Project = project;
            Params = Project.Configuration;
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
