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
        public string Name { get; } // Effect name
        public double Intensity { get; set; }// Effect intensity from 0 to 1
        public string LabelIntensity { get;}// Actual intensity to display
        public TimeSpan StartTime { get; set; }// Effect start time relative to file start
        public TimeSpan EndTime { get; set; }// Effect end time relative to file start
        public string GetArgs();//Get arguments for ffmpeg
        
    }
}
