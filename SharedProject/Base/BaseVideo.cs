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

namespace SharedProject.Base
{
    public class BaseVideo<TTexture, TRenderTarget> : BaseGLClass, IVideo
        where TRenderTarget : BaseGLClass, IRenderTarget<TRenderTarget>
        where TTexture : BaseGLClass, ITexture<TTexture>
    {
        private Image<Rgba32> video = default!;
        private SharedResProject.Shader ShaderCentrloids = default!;
        private DrawBuffer DrawBuffer = default!;
        public TRenderTarget RenderTarget = default!;

        public TTexture Texture { get; set; } = default!;

        public int FrameCount { get; set; }
        public bool IsNaNAbleKMeans { get; set; }

        public float[,] KMeans { get; set; } = new float[3, 3] { { 0.7f, 0.2f, 0.5f }, { 1f, 0.5f, 0.7f }, { 0.5f, 0.7f, 0.2f } };
        public int FramePosition { get; set; } = 0;
        public BaseVideo(GL gl, string path, InternalFormat internalFormat = InternalFormat.Rgb) : base(gl, true)
        {
            Init(path, internalFormat);
        }

        public virtual unsafe void Init(string path, InternalFormat internalFormat)
        {
            ShaderCentrloids = new SharedResProject.Shader(Gl, "centroidCal");
            DrawBuffer = new DrawBuffer(Gl);

            video = Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);
            FrameCount = video.Frames.Count;

            Texture = TTexture.Init(Gl, video.Frames[FramePosition], internalFormat);

            RenderTarget = TRenderTarget.Init(Gl, Texture.Height, Texture.Width, 3, internalFormat);
            BindAndApplyShader();
        }

        public void NextFrame()
        {
            FramePosition++;
            if (FrameCount <= FramePosition)
                FramePosition = 0;

            var img = video.Frames[FramePosition];

            if (img.PixelBuffer is not null)
            {
                if (Texture is not null)
                {
                    Texture.Dispose();
                }

                Texture = TTexture.Init(Gl, img);
            }
        }
        public void GetFrame(int position)
        {
            var positionInter = position;
            if (FrameCount <= positionInter || positionInter < 0)
                positionInter = 0;

            var img = video.Frames[positionInter];

            if (img.PixelBuffer is not null)
            {
                if (Texture is not null)
                {
                    Texture.Dispose();
                }

                Texture = TTexture.Init(Gl, img);
            }
        }
        public void Dispose()
        {
            video.Dispose();
            Texture.Dispose();
            ShaderCentrloids.Dispose();
            DrawBuffer.Dispose();
            RenderTarget.Dispose();
        }

        public unsafe void BindAndApplyShader()
        {
            //Gl.Clear(ClearBufferMask.ColorBufferBit);
            RenderTarget.Bind();
            DrawBuffer.Bind();
            ShaderCentrloids.Use();
            Texture.Bind();
            ShaderCentrloids.SetUniform("uTexture0", 0);
            ShaderCentrloids.SetUniform3("cent1", KMeans[0, 0], KMeans[0, 1], KMeans[0, 2]);
            ShaderCentrloids.SetUniform3("cent2", KMeans[1, 0], KMeans[1, 1], KMeans[1, 2]);
            ShaderCentrloids.SetUniform3("cent3", KMeans[2, 0], KMeans[2, 1], KMeans[2, 2]);

            Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);
            RenderTarget.UnBind();
            KMeans = RenderTarget.RecalculateAndGetAvrColor();

            if (IsNaNAbleKMeans)
            {
                KMeansUnsetNaN();
            }
        }

        public void KMeansUnsetNaN()
        {
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    if (float.IsNaN(KMeans[x, y]))
                    {
                        KMeans[x, y] = x - 1;
                    }
                }
            }
        }

    }
}
