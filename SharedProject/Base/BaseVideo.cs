using SharedResProject;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject.Interface;
using Silk.NET.SDL;
using Silk.NET.Vulkan.Video;
using SharedProject.Interface.HaveAtributes;
using System.Reflection.Metadata;

namespace SharedProject.Base
{
    public abstract class BaseVideo : BaseGLClass, IVideo
    {
        public ITexture Texture { get; set; } = default!;
        public InternalFormat InternalFormat { get; } = default!;
        public int FramePosition { get; set; } = 0;
        public BaseVideo(GL gl, string path, InternalFormat internalFormat,uint renderTargetSize) : base(gl)
        {
            InternalFormat = internalFormat;
            Init(path, internalFormat, renderTargetSize);
        }
        public abstract unsafe void Init(string path, InternalFormat internalFormat, uint renderTargetSize);
        public abstract void NextFrame();
        public override void Dispose()
        {
            Texture?.Dispose();

            base.Dispose();
        }
    }
}
