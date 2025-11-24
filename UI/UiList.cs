using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditor.Core;
using VideoEditor.Core.Effects;
using VideoEditor.Core.Transitions;
namespace Video_Editor
{
    public static class UiList
    {
        private static readonly List<IEffect> EffectsList = [new BrightnessEffect(), new FadeEffect(), new GrayScaleEffect()];
        public static List <String> EffectsNames= EffectsList.Select(o=>o.Name).ToList();
        public static IEffect NewEffect(string name)
        {
            foreach(var effect in EffectsList)
            {
                if (effect.Name == name) return (IEffect)Activator.CreateInstance(effect.GetType());
            }
            return null;
        }

        private static readonly List<ITransition> TransitionsList= [new CrossFadeTransition(),
                                                        new DissolveTransition(),
                                                        new FadeBlackTransition(),
                                                        new SlideLeftTransition(),
                                                        new SlideRightTransition()];
        public static List<String>TransitionNames= TransitionsList.Select(o=>o.Name).ToList();
        public static ITransition NewTransition(string name)
        {
            foreach(var transition in TransitionsList)
            {
                if(transition.Name == name) return (ITransition)Activator.CreateInstance(transition.GetType());
            }
            return null;
        }
        public static readonly List<TextFragment> Selectables = [new TextFragment("Simple text", "Times New Roman", 48, Color.White),
                                                        new TextFragment("Title", "Arial", 96, Color.White),
                                                        new TextFragment("Highlight", "Impact", 72, Color.Yellow)];
        public static TextFragment NewText(string name)
        {
            foreach(var text in Selectables)
            {
                if (name == text.Text)
                {
                    var newText = text.DeepCopy();
                    newText.Name = name;
                    return (TextFragment)newText;
                }
            }
            return null;
        }
    }
}
