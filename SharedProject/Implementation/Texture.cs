using SharedProject.Base;
using SharedProject.Interface;
using SharedProject.Interface.Atomic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Implementation
{
    public class Texture : BaseTexture
    {
        public Texture(GL gl, string path, InternalFormat internalFormat) : base(gl, path, internalFormat)
        {
        }

        public Texture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl, img, internalFormat, disposeImg)
        {
        }

        public unsafe Texture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null) : base(gl, data, width, height, internalFormat, pixelFormat, action)
        {
        }
    }
}
