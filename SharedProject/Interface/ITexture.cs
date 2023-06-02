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
    public interface ITexture : IDisposable, IBindAble, IMesureAble, IHandlerAble,IAvgAble
    {
        void Bind(TextureUnit textureSlot);
        unsafe void ChangeContent(void* data);
        int TotalMipmapLevels { get; set; }
    }
}
