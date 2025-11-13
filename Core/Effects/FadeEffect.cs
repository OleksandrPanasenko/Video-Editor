using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Effects
{
    public class FadeEffect : IEffect
    {
        public string Name { get => "Fade";}
        public double Intensity { get; set; } = 0;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string LabelIntensity => Intensity < 0.5 ? "Fade In" : "Fade Out";

        public string GetArgs()
        {
            return Intensity<0.5?"fade=in": "fade=out";
        }
    }
}
