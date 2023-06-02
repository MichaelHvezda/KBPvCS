using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface
{
    public interface IVideo : IDisposable
    {
        ITexture Texture { get; set; }
        int FramePosition { get; set; }
        void NextFrame();
    }
}
