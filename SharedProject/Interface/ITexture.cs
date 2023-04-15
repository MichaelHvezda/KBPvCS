using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject.Interface.Atomic;
using Silk.NET.Maths;
using SharedProject.Base;
using Silk.NET.SDL;

namespace SharedProject.Interface
{
    public interface ITexture : IDisposable, IBindAble, IMesureAble, IHandlerAble
    {
        static abstract ITexture Init(GL gl,Span<float> data, uint width, uint height, InternalFormat internalFormat);
        static abstract ITexture Init(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false);
        void RecalculateAvrColor();
        public Vector4D<float> AvgColor { get; set; }

        void Bind(TextureUnit textureSlot);
    }
}
