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
        IRenderTarget RenderTarget { get; set; }
        ITexture Texture { get; set; }
        float[,] KMeans { get; set; }
        bool IsNaNAbleKMeans { get; set; }
        int FramePosition { get; set; }
        void NextFrame();
        void BindAndApplyShader();
        static abstract IVideo Init(GL gl, string path, InternalFormat internalFormat);
    }
}
