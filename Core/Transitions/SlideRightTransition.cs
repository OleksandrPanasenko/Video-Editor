using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core.Transitions
{
    public class SlideRightTransition : ITransition
    {
        public TimeSpan Duration { get; set; }
        public FragmentPlacement From { get; set; }
        public FragmentPlacement To { get; set; }

        public string Name => "Slide Right";

        public string GetArgs()
        {
            throw new NotImplementedException();
        }
    }
}
