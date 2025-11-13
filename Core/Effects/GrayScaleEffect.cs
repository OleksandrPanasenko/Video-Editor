using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Effects
{
    public class GrayScaleEffect : IEffect
    {
        public string Name { get => "GrayScale"; }
        public double Intensity { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string LabelIntensity => $"{Intensity*100}%";

        public string GetArgs()
        {
            return $"colorchannelmixer="+
                $"rr={((1 - Intensity) + Intensity * 0.33).ToString(CultureInfo.InvariantCulture)}:"+
                $"gg={((1 - Intensity) + Intensity * 0.33).ToString(CultureInfo.InvariantCulture)}:"+
                $"bb={((1 - Intensity) + Intensity * 0.33).ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
