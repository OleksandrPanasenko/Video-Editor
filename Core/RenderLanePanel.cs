using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using VideoEditor.Core.Effects;
namespace VideoEditor.Core
{
    internal class RenderLanePanel
    {
        public Project Project { get; set; }
        internal ProjectConfig Params;
        private Graphics g;
        private static List<double> NiceIntervals = [0.01, 0.02, 0.05, 0.1, 0.2, 0.5, 1, 2, 5, 10, 15, 20, 30, 60, 120, 300, 600, 900, 1800, 3600];
        public RenderLanePanel(Project project, Graphics g)
        {
            Project = project;
            Params = Project.Configuration;
            this.g = g;
        }
        public void Render()
        {
            int yUpperBound = (int)Params.LanePanelScrollY;
            int yLowerBound = yUpperBound + Params.LanePanelHeight - Params.TimeRulerHeight + Params.LaneSpacing;
            for (int i = 0, y = 0; i < Project.Lanes.Count; i++, y += Params.LaneHeight + Params.LaneSpacing)
            {
                if (y + Params.LaneHeight < yUpperBound) continue;
                if (y > yLowerBound) break;
                RenderLane(Project.Lanes[i], i);
            }
            RenderLaneLabels();
            RenderTimeRuler();
            RenderTimeMarker(Project.SelectionManager.SelectedTime);
        }
        internal void RenderLane(Lane lane, int laneIndex)
        {
            //render lane background
            using (var brush = (new SolidBrush(lane == Project.SelectionManager.SelectedLane ? Config.LaneBackgroundColor : Config.LaneSelectedBackgroundColor)))

            using (var pen = new Pen(Config.LaneBorderColor))
            {
                Rectangle rect = (new Rectangle(0 + Params.LaneLabelWidth, (int)(laneIndex * (Params.LaneHeight + Params.LaneSpacing) - Params.LanePanelScrollY + Params.TimeRulerHeight), Params.LanePanelWidth - Params.LaneLabelWidth, Params.LaneHeight));
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect);
            }

            //render lane border
            foreach (var fragment in lane.Fragments)
            {
                if (fragment.EndPosition < TimeSpan.FromSeconds(Params.LanePanelScrollX / Params.LaneTimeScale)) continue;
                if (fragment.Position > TimeSpan.FromSeconds((Params.LanePanelScrollX + Params.LanePanelWidth - Params.LaneLabelWidth) / Params.LaneTimeScale)) break;
                RenderFragment(laneIndex, fragment);
            }
            //render fragments
        }
        internal void RenderFragment(int laneIndex, FragmentPlacement fragment)
        {
            Color fragmentColor = fragment.Fragment.FragmentType switch
            {
                Fragment.Type.Video => Config.VideoBlockColor,
                Fragment.Type.Audio => Config.AudioBlockColor,
                Fragment.Type.Text => Config.TextBlockColor,
                Fragment.Type.Image => Config.ImageBlockColor,
                _ => Config.VideoBlockColor
            };
            //render fragment background
            using (var brush = new SolidBrush(fragmentColor))
            using (var pen = new Pen(Config.LaneBorderColor))
            using (var textBrush = new SolidBrush(Color.Black))
            {
                var rectX = (int)(fragment.Position.TotalSeconds * Params.LaneTimeScale - Params.LanePanelScrollX + Params.LaneLabelWidth);
                var rectY = (int)(laneIndex * (Params.LaneHeight + Params.LaneSpacing) - Params.LanePanelScrollY + Params.TimeRulerHeight);
                var rectWidth = (int)(fragment.Fragment.Duration.TotalSeconds * Params.LaneTimeScale);
                var rectHeight = Params.LaneHeight;

                Rectangle rect = new Rectangle(
                rectX, rectY, rectWidth, rectHeight);
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect);

                if (fragment == Project.SelectionManager.SelectedFragment)
                {
                    using (var selectFrame = new Pen(Config.SelectionHighlightColor, 2))
                    {
                        g.DrawRectangle(selectFrame, rect);
                    }
                }


                //render fragment name
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                g.DrawString(fragment.Fragment.Name, Config.FragmentNameFont, textBrush, rect, format);
                var numEffects = fragment.Fragment.Effects.Count;
                for (int i = 0; i < numEffects; i++)
                {
                    IEffect effect = fragment.Fragment.Effects[i];
                    var effectHeight = rectHeight / numEffects;

                    var effectY = rectY + i * effectHeight;
                    var effectX = Math.Max(TimeToX(fragment.Position), TimeToX(fragment.Position - fragment.Fragment.StartTime + effect.StartTime));
                    var effextXEnd= Math.Min(TimeToX(fragment.EndPosition), TimeToX(fragment.Position - fragment.Fragment.StartTime + effect.EndTime));
                    //Fix color
                    using (var selectFrame = new Pen(Config.SelectionHighlightColor, 2))
                    {
                        g.DrawRectangle(selectFrame, rect);
                    }
                }
            }
        }
        //render fragment border
        //render effect lines


    

        internal void RenderTimeMarker(TimeSpan? time)
        {
            if (time == null) return;
            TimeSpan? Time = Project.SelectionManager.SelectedTime;
            //render time marker line
            if (Time != null)
            {
                using (var Brush=new SolidBrush(Config.TimePointerColor)) { 
                    Rectangle rect = new Rectangle(
                        (int)(TimeToX((TimeSpan)Time) - (Params.TimeMarkerWidth / 2)),
                        Params.TimeRulerHeight,
                        Params.TimeMarkerWidth,
                        Params.LanePanelHeight-Params.TimeRulerHeight);
                    g.FillRectangle(Brush, rect);

                    //render time label
                    using (Font font = new Font("Segoe UI", Params.TimeRulerHeight / 3 / 2,FontStyle.Bold))
                    {
                        string text = Time.ToString();
                        var size = g.MeasureString(text, font);
                        g.DrawString(text, font, Brush, (int)TimeToX((TimeSpan)Time) - size.Width / 2, Params.TimeRulerHeight/2);
                    }
                }
            }
            

        }
        internal void RenderLaneLabels()
        {
            //render lane labels on the left side
            using(var brush= new SolidBrush(Config.LanePanelBackgroundColor))
            {
                g.FillRectangle(brush,new Rectangle(0,Params.TimeRulerHeight, Params.LaneLabelWidth, Params.LanePanelHeight));
                //TODO for (int )
            }
        }
        internal void RenderTimeRuler()
        {
            double interval = GetInterval(Config.TimeRulerMinDistancePixels);
            using (Pen pen=new Pen(Config.TimeRulerColor))
            using (Font font= new Font("Segoe UI", Params.TimeRulerHeight / 3 / 2))
            using (var brush= new SolidBrush(Config.TimeRulerColor))
            {
                for (double t = EarliestMark(interval); t < XToTime(Params.LanePanelWidth+ Params.LanePanelScrollX).TotalSeconds; t += interval)
                {
                    int x = (int)TimeToX(TimeSpan.FromSeconds(t));
                    g.DrawLine(pen, x, 0, x, Params.TimeRulerHeight * 2 / 3);
                    string text = FormatTimeLabel(t);
                    var size=g.MeasureString(text, font);
                    g.DrawString(text, font, brush, x-size.Width/2, 0);
                }
                for (double t = EarliestMark(interval/5); t < XToTime(Params.LanePanelWidth).TotalSeconds; t += interval/5)
                {
                    int x = (int)TimeToX(TimeSpan.FromSeconds(t));
                    g.DrawLine(pen, x, 0, x, Params.TimeRulerHeight * 1 / 3);
                }
            }
        }
        internal double GetInterval(double targetPixels)
        {
            double targetSeconds = targetPixels / Params.LaneTimeScale;
            foreach(var interval in NiceIntervals)
            {
                if(interval>=targetSeconds) return interval;
            }
            return NiceIntervals.Last();
        }
        internal double TimeToX(TimeSpan time)
        {
            return time.TotalSeconds * Params.LaneTimeScale - Params.LanePanelScrollX + Params.LaneLabelWidth;
        }
        internal TimeSpan XToTime(double x)
        {
            return TimeSpan.FromSeconds((x + Params.LanePanelScrollX - Params.LaneLabelWidth) / Params.LaneTimeScale);
        }
        internal double EarliestMark(double interval)//earliest time mark to render (seconds)
        {
            double startSeconds = Params.LanePanelScrollX / Params.LaneTimeScale;
            return Math.Ceiling(startSeconds / interval) * interval;
        } 
        internal string FormatTimeLabel(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            if (time.TotalHours >= 1)
                return time.ToString(@"hh\:mm\:ss");
            else
                return time.ToString(@"mm\:ss");
        }

    }
}
