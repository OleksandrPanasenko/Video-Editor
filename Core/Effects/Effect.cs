using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoEditor.Core.Effects
{
    //Prevent JSON deserialization error
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(BrightnessEffect), typeDiscriminator: "Bright")]
    [JsonDerivedType(typeof(FadeEffect), typeDiscriminator: "Fade")]
    [JsonDerivedType(typeof(GrayScaleEffect), typeDiscriminator: "GrayScale")]
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
