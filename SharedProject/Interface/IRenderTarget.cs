using SharedProject.Interface.Atomic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface
{
    public interface IRenderTarget : IDisposable, IDrawAble, IBindAble, IMesureAble, IHandlerAble
    {
        public GLEnum[] DrawBuffers { get; set; }
        void Init(uint Height, uint Width, uint Count, InternalFormat internalFormat);
    }
    public interface IAvrRenderTarget : IRenderTarget
    {
        Vector3D<float>[] RecalculateAndGetAvrColor();
    }
}
