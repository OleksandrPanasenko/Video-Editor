using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    internal class ProjectConfig
    {
        public int WindowWidth { get; set; } = 1280;
        public int WindowHeight { get; set; } = 720;
        public int WindowX { get; set; } = 0;
        public int WindowY { get; set; } = 0;
        public int LeftTabIndex { get; set; } = 0;
        public int RightTabIndex { get; set; } = 0;
        public int LanePanelWidth { get; set; } = 1000;
        public int LanePanelHeight { get; set; } = 400;
        public double LanePanelScrollX { get; set; } = 0;
        public double LanePanelScrollY { get; set; } = 0;
        public int LaneHeight { get; set; } = 100;
        public int LaneSpacing { get; set; } = 10;
        public int LaneTimeScale { get; set; } = 100; // pixels per second
        public int LaneLabelWidth { get; set; } = 100;
        public int TimeRulerHeight { get; set; } = 20;
    }
}
