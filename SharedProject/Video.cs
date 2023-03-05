using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using System.Reflection.Metadata;
using SixLabors.ImageSharp.Formats;
using Silk.NET.Maths;
using SixLabors.ImageSharp.Processing;

namespace SharedResProject
{
    public class Video
    {
        private GL gl;
        private readonly string path;
        private Image<Rgba32> video;
        private static Shader ShaderCentrloids;
        private static DrawBuffer DrawBuffer;
        public static RenderTarget RenderTarget;

        public Texture Texture { get; set; } = default!;

        public int FrameCount { get; set; }

        public float[,] KMeans { get; set; } = new float[3, 3] { { 0.7f, 0.2f, 0.5f }, { 1f, 0.5f, 0.7f }, { 0.5f, 0.7f, 0.2f } };
        public int FramePosition { get; set; } = 0;
        public Video(GL gl, string path)
        {
            this.gl = gl;
            this.path = path;

            ShaderCentrloids = new Shader(gl, "centroidCal");
            DrawBuffer = new DrawBuffer(gl);

            video = Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);
            FrameCount = video.Frames.Count;
            //video.Mutate(p => p.Resize(video.Width / 8, video.Height / 8));

            Texture = new Texture(gl, video.Frames[FramePosition]);

            RenderTarget = new RenderTarget(gl, Texture.Height, Texture.Width, 3);
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

                Texture = new Texture(gl, img);
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

                Texture = new Texture(gl, img);
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
            //gl.Clear(ClearBufferMask.ColorBufferBit);
            RenderTarget.Bind();
            DrawBuffer.Bind();
            ShaderCentrloids.Use();
            Texture.Bind(TextureUnit.Texture0);
            ShaderCentrloids.SetUniform("uTexture0", 0);
            ShaderCentrloids.SetUniform3("cent1", KMeans[0, 0], KMeans[0, 1], KMeans[0, 2]);
            ShaderCentrloids.SetUniform3("cent2", KMeans[1, 0], KMeans[1, 1], KMeans[1, 2]);
            ShaderCentrloids.SetUniform3("cent3", KMeans[2, 0], KMeans[2, 1], KMeans[2, 2]);

            gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);
            RenderTarget.UnBind();
            KMeans = RenderTarget.RecalculateAndGetAvrColor();
        }

    }
}
