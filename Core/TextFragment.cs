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
        public string Text { get; set; }
        public string FontName { get; set; } = "Arial";
        public int FontSize { get; set; } = 24;
        public Color TextColor { get; set; } = Color.White;
        public Point Position { get; set; } = new Point(100, 100);
        public override Type FragmentType => Type.Text;

    }
}
