using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Effects
{
    public class BrightnessEffect : IEffect
    {
        public string Name { get => "Brightness";}
        public double Intensity { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string LabelIntensity => $"{Intensity*2-1}";

        public string GetArgs()
        {
            return $"eq=brightness={(Intensity * 0.5 - 0.25).ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
