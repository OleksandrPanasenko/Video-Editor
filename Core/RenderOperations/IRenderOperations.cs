using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Core
{
    public interface IRenderOperations
    {
        public string Name { get; }
        public string FfmpegDesription();
    }
    public interface IRenderProject:IRenderOperations { 
        public string FFMpegDesription(Project project);
    }
    public interface IRenderLane : IRenderOperations
    {
        public string FFMpegDesription(Lane lane);
    }
}
