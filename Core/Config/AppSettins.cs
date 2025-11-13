using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public class AppSettings
    {

        public string DefaultProjectFolder { get; set; }
        public string FfmpegPath { get; set; }
        public string FfprobePath { get; set; }
        public bool AutoSave { get; set; }
        public int AutoSaveTimeMinutes { get; set; }
        public Color LaneColor { get; set; }
        public Color LaneBackgroundColor { get; set; }
        public Color LabelPanelColor { get; set; }
        public Color TimeStamp { get; set; }
        public Color VideoFragmentColor { get; set; }
        public Color AudioFragmentColor { get; set; }
        public Color ImageFragmentColor { get; set; }
        public Color TextFragmentColor { get; set; }
        public AppSettings()
        {

        }
        public void ApplySettings()
        {
            Config.AutoSave = this.AutoSave;
            Config.AutoSaveIntervalMinutes = this.AutoSaveTimeMinutes;
            Config.LaneBackgroundColor = this.LaneColor;
            Config.LanePanelBackgroundColor = this.LaneBackgroundColor;

            //Config.
            Config.TimePointerColor = this.TimeStamp;
            Config.TimeRulerColor = this.TimeStamp;

            Config.VideoBlockColor = this.VideoFragmentColor;
            Config.AudioBlockColor = this.AudioFragmentColor;
            Config.ImageBlockColor = this.ImageFragmentColor;
            Config.TextBlockColor = this.TextFragmentColor;
        }
        public static AppSettings Default = new AppSettings();
        public static void SaveDefaults()
        {
            Default = FromSettings();

        }
        public static AppSettings FromSettings()
        {
            AppSettings newSettings = new AppSettings();

            newSettings.AutoSave = Config.AutoSave;
            newSettings.TextFragmentColor = Config.TextBlockColor;
            newSettings.ImageFragmentColor = Config.ImageBlockColor;
            newSettings.AudioFragmentColor = Config.AudioBlockColor;
            newSettings.VideoFragmentColor = Config.VideoBlockColor;

            newSettings.TimeStamp = Config.TimeRulerColor;
            newSettings.TimeStamp = Config.TimePointerColor;

            //Config.LaneLabelBackgroundColor
            newSettings.LaneBackgroundColor = Config.LanePanelBackgroundColor;
            newSettings.LaneColor = Config.LaneBackgroundColor;
            newSettings.AutoSaveTimeMinutes = Config.AutoSaveIntervalMinutes;

            return newSettings;
        }
    }
}
