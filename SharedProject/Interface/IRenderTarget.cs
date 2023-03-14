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
    public interface IRenderTarget<TRenderTarget> : IDisposable, IDrawAble, IBindAble, IMesureAble, IHandlerAble
    {
        void Init(uint Height, uint Width, uint Count, InternalFormat internalFormat = InternalFormat.Rgb);
        float[,] RecalculateAndGetAvrColor();
        static abstract TRenderTarget Init(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat);
    }
}
