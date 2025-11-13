using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Effects
{
    public interface IEffect
    {
        public string Name { get; }
        public double Intensity { get; set; }
        public string LabelIntensity { get;}
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string GetArgs();
        
    }
}
