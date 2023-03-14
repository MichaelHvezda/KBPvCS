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

namespace SharedProject.Interface
{
    public interface ITexture<TTexture> : IDisposable, IBindAble, IMesureAble, IHandlerAble
    {
        void Init(string path, InternalFormat internalFormat);
        void Init(ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false);
        void Init(Span<float> data, uint width, uint height, InternalFormat internalFormat);
        static abstract TTexture Init(GL gl,Span<float> data, uint width, uint height, InternalFormat internalFormat);
        static abstract TTexture Init(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat = InternalFormat.Rgb8, bool disposeImg = false);
        void RecalculateAvrColor();
        public Vector4D<float> AvgColor { get; set; }
    }
}
