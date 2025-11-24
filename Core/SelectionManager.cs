using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
        public TimeSpan? DragStartTime { get; set; }
        public Lane? SelectedLane { get; set; }

        public FragmentPlacement? MemoryFragment { get; set; }
        public enum EditAction {None, Move, Split, TrimStart, TrimEnd }
        public EditAction PendingEdit { get; set; }= EditAction.None;

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
                int laneIndex = (int)((y + Params.LanePanelScrollY-Params.TimeRulerHeight) / (Params.LaneHeight + Params.LaneSpacing));
                if (laneIndex >= Project.Lanes.Count)
                {
                    ClearSelection();
                    return;
                }
                SelectedLane = Project.Lanes[laneIndex];
                if (x > Params.LaneLabelWidth)
                {

                    TimeSpan time = TimeSpan.FromSeconds((x + Params.LanePanelScrollX - Params.LaneLabelWidth) / Params.LaneTimeScale);// Maybe make ScrollX in seconds?
                    SelectedTime = time;
                    SelectedFragment = SelectedLane[time];

                    if (SelectedFragment == null&&time!=null)
                    {
                        var transition= SelectedLane.GetTransitionFromTime(time);
                        if (transition != null&&transition.From!=null&&transition.To!=null)
                        {
                            double yCoef = ((y + Params.LanePanelScrollY - Params.TimeRulerHeight) / (Params.LaneHeight + Params.LaneSpacing))- laneIndex;
                            double xCoef = (time.TotalSeconds - transition.From.EndPosition.TotalSeconds) / (transition.Duration.TotalSeconds);
                            if (yCoef < (1-xCoef))
                            {
                                SelectedFragment = transition.From;
                            }
                            else
                            {
                                SelectedFragment = transition.To;
                            }
                        }
                    }
                }
                else
                {
                    SelectedFragment=null;
                    SelectedTime=null;
                }
            }
        }
        public void SelectTime(int x, int y)
        {
            if (y > Params.TimeRulerHeight || x > Params.LaneLabelWidth || y < Params.LanePanelHeight || x < Params.LanePanelWidth)
            {
                TimeSpan time = TimeSpan.FromSeconds((x + Params.LanePanelScrollX - Params.LaneLabelWidth) / Params.LaneTimeScale);// Maybe make ScrollX in seconds?
                SelectedTime = time;
            }
        }
        public void SelectObject(Point point)
        {
            SelectObject(point.X, point.Y);
        }
    }
}
