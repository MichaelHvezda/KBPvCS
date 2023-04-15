using SharedProject.Interface.Atomic;
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
        public ITexture[] ColorBuffers { get; set; }
        public GLEnum[] DrawBuffers { get; set; }
        void Init(uint Height, uint Width, uint Count, InternalFormat internalFormat);
        float[,] RecalculateAndGetAvrColor();
        static abstract IRenderTarget Init(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat);
    }
}
