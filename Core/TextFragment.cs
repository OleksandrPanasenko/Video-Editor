using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public class TextFragment: Fragment
    {
        public TextFragment() { }
        public TextFragment(string text, string fontName, int fontSize, Color fontColor)
        {
            this.Text = text;
            this.TextColor = fontColor;
            this.FontSize = fontSize;
            this.FontName = fontName;
        }
        public string Text { get; set; }
        public string FontName { get; set; } = "Arial";
        public int FontSize { get; set; } = 24;
        public Color TextColor { get; set; } = Color.White;
        public Point Position { get; set; } = new Point(100, 100);
        public override Type FragmentType => Type.Text;
        public override Fragment DeepCopy()
        {
            TextFragment newFragment = new TextFragment();
            newFragment.StartTime= this.StartTime;
            newFragment.EndTime= this.EndTime;
            newFragment.Width = Width;
            newFragment.Height = Height;
            newFragment.Opacity = Opacity;
            newFragment.FileDuration = FileDuration;
            newFragment.Text = Text;
            newFragment.TextColor = TextColor;
            newFragment.FontSize = FontSize;
            newFragment.FontName = FontName;


            return newFragment;
        } 

    }
}
