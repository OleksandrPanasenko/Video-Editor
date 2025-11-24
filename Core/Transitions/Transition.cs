using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoEditor.Core.Transitions
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(CrossFadeTransition), "crossfade")]
    [JsonDerivedType(typeof(DissolveTransition), "dissolve")]
    [JsonDerivedType(typeof(FadeBlackTransition), "fadeblack")]
    [JsonDerivedType(typeof(SlideLeftTransition), "slideleft")]
    [JsonDerivedType(typeof(SlideRightTransition), "slideright")]
    public interface ITransition
    {
        public TimeSpan Duration { get; set; }
        public FragmentPlacement From { get; set; }
        public FragmentPlacement To { get; set; }
        public string Name { get; }
        public string GetArgs();
    }
}
