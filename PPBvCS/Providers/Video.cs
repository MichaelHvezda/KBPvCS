using Emgu.CV;
using SharedProject.Implementation;
using SharedProject.Interface;
using SharedProject.Video;
using SharedResProject;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBvCS.Providers
{
    public sealed class VLVideo : AvgVLVideo //AvrSLVideo //AvgEMGUVideo
    {
        public VLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public uint BGTextureId { get; set; } = 0;

        public uint GetBGTextureId(float bgVal)
        {
            List<(float, uint)> values = new()
            {
                (Math.Abs(KMeans[0][0] - bgVal), 0),
                (Math.Abs(KMeans[1][0] - bgVal), 1),
                (Math.Abs(KMeans[2][0] - bgVal), 2)
            };

            return values.OrderBy(p => p.Item1).First().Item2;
        }
        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            VideoData = new VideoLoader(path);
            LoadFrame();
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "../../../../../shaders/centroidCal");

            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, InternalFormat.Rgba16f);
        }

        protected unsafe override ITexture CreateTexture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat)
        {
            return new Texture(gl, data, width, height, internalFormat, Silk.NET.OpenGL.PixelFormat.Rgba);
        }

    }

    public class EMGUVideo : AvgEMGUVideo //AvrSLVideo //AvgEMGUVideo
    {
        public EMGUVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public uint BGTextureId { get; set; } = 0;

        public uint GetBGTextureId(float bgVal)
        {
            List<(float, uint)> values = new()
            {
                (Math.Abs(KMeans[0][0] - bgVal), 0),
                (Math.Abs(KMeans[1][0] - bgVal), 1),
                (Math.Abs(KMeans[2][0] - bgVal), 2)
            };

            return values.OrderBy(p => p.Item1).First().Item2;
        }
        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            filePath = path ?? string.Empty;
            VideoData = new VideoCapture(path);
            LoadFrame();
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "../../../../../shaders/centroidCal");

            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, InternalFormat.Rgba16f);
        }
    }
    public class SLVideo : AvrSLVideo //AvrSLVideo //AvgEMGUVideo
    {
        public SLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public uint BGTextureId { get; set; } = 0;

        public uint GetBGTextureId(float bgVal)
        {
            List<(float, uint)> values = new()
            {
                (Math.Abs(KMeans[0][0] - bgVal), 0),
                (Math.Abs(KMeans[1][0] - bgVal), 1),
                (Math.Abs(KMeans[2][0] - bgVal), 2)
            };

            return values.OrderBy(p => p.Item1).First().Item2;
        }
        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            VideoData = SixLabors.ImageSharp.Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);
            FrameCount = VideoData.Frames.Count;
            Texture = CreateTexture(Gl, VideoData.Frames[FramePosition], internalFormat);
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "../../../../../shaders/centroidCal");

            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, InternalFormat.Rgba16f);
        }
    }
}
