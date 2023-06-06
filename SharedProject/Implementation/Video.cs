using SharedProject.Base;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResProject;
using SharedProject.Interface;
using Silk.NET.SDL;
using Emgu.CV;
using SharedProject.Interface.HaveAtributes;
using Silk.NET.Maths;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using SharedProject.Video;

namespace SharedProject.Implementation
{
    public class SLVideo : BaseVideo
    {
        public int FrameCount { get; set; }
        public Image<Rgba32> VideoData { get; set; } = default!;
        public SLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            VideoData = Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);
            FrameCount = VideoData.Frames.Count;
            Texture = CreateTexture(Gl, VideoData.Frames[FramePosition], internalFormat);
        }

        internal unsafe virtual ITexture CreateTexture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat)
        {
            return new Texture(gl, img, internalFormat);
        }


        public override void NextFrame()
        {
            FramePosition++;
            if (FrameCount <= FramePosition)
                FramePosition = 0;

            var img = VideoData.Frames[FramePosition];

            if (img.PixelBuffer is not null)
            {
                if (Texture is null)
                {
                    Texture = CreateTexture(Gl, img, InternalFormat.Rgba8);
                }

                Texture.ChangeContent(img);
            }
        }

        public override void Dispose()
        {
            VideoData?.Dispose();
            base.Dispose();
        }
    }
    public class EMGUVideo : BaseVideo
    {
        public VideoCapture VideoData { get; set; } = default!;
        private string filePath { get; set; } = string.Empty;

        public EMGUVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            filePath = path ?? string.Empty;
            VideoData = new VideoCapture(path);
            LoadFrame();
        }


        internal unsafe virtual ITexture CreateTexture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat)
        {
            return new Texture(gl, data, width, height, internalFormat, Silk.NET.OpenGL.PixelFormat.Rgb);
        }

        private unsafe void LoadFrame()
        {
            //var time = DateTime.Now;
            using var mat = new Mat();
            if (VideoData.Read(mat))
            {

                //Console.WriteLine((long)VideoData.Ptr);
                if (Texture is null)
                {
                    //Console.WriteLine("texture cscd {0}", (time - DateTime.Now).TotalMilliseconds);
                    Texture = CreateTexture(Gl, mat.DataPointer.ToPointer(), (uint)mat.Width, (uint)mat.Height, InternalFormat);
                    return;
                }

                //Console.WriteLine("texture readed {0}", (time - DateTime.Now).TotalMilliseconds);

                Texture.ChangeContent(mat.DataPointer.ToPointer());
                //Console.WriteLine("texture creationdada {0}", (time - DateTime.Now).TotalMilliseconds);
                return;
            }
            FramePosition = 0;
            if (VideoData.Set(CapProp.PosFrames, FramePosition) && VideoData.Read(mat))
            {
                Texture.ChangeContent(mat.DataPointer.ToPointer());
            }



            //var time = DateTime.Now;
            //VideoData.Dispose();
            //Console.WriteLine("dispose {0}", (time - DateTime.Now).TotalMilliseconds);
            //VideoData = new VideoCapture(filePath);
            //Console.WriteLine("create {0}", (time - DateTime.Now).TotalMilliseconds);

            //VideoData.Stop();

            //Console.WriteLine("texture creation {0}", (time - DateTime.Now).TotalMilliseconds);
        }


        public override void NextFrame()
        {
            FramePosition++;
            LoadFrame();
        }
        public override void Dispose()
        {
            VideoData?.Dispose();
            base.Dispose();
        }
    }
    public class VLVideo : BaseVideo
    {
        public VideoLoader VideoData { get; set; } = default!;
        public VLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            VideoData = new VideoLoader(path);
            LoadFrame();
        }

        internal unsafe virtual ITexture CreateTexture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat)
        {
            return new Texture(gl, data, width, height, internalFormat, Silk.NET.OpenGL.PixelFormat.Rgba);
        }

        private unsafe void LoadFrame()
        {
            //var time = DateTime.Now;
            if (VideoData.Load(out var bytes))
            {

                //Console.WriteLine((long)VideoData.Ptr);
                if (Texture is null)
                {
                    //Console.WriteLine("texture cscd {0}", (time - DateTime.Now).TotalMilliseconds);
                    fixed (void* d = &bytes[0])
                        Texture = CreateTexture(Gl, d, (uint)VideoData.Width, (uint)VideoData.Height, InternalFormat);
                    return;
                }

                //Console.WriteLine("texture readed {0}", (time - DateTime.Now).TotalMilliseconds);
                fixed (void* d = &bytes[0])
                    Texture.ChangeContent(d);
                //Console.WriteLine("texture creationdada {0}", (time - DateTime.Now).TotalMilliseconds);
                return;
            }
            FramePosition = 0;
            if (VideoData.LoadFirst(out var bytesFirst))
            {
                fixed (void* d = &bytesFirst[0])
                    Texture.ChangeContent(d);
            }
        }

        public override void NextFrame()
        {
            FramePosition++;
            LoadFrame();
        }

        public override void Dispose()
        {
            VideoData?.Dispose();
            base.Dispose();
        }
    }

    public class AvrSLVideo : SLVideo, IKMeansAble, IShaderAble
    {
        public Vector3D<float>[] KMeans { get; set; }
        public SharedResProject.Shader Shader { get; set; }
        public virtual AvrRenderTarget RenderTarget { get; set; }
        public DrawBuffer DrawBuffer { get; set; } = default!;
        public bool IsNaNAbleKMeans { get; set; } = true;

        public AvrSLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
            KMeans = new Vector3D<float>[3] { new Vector3D<float>(0.7f, 0.2f, 0.5f), new Vector3D<float>(1f, 0.5f, 0.7f), new Vector3D<float>(0.5f, 0.7f, 0.2f) };
        }

        public unsafe void BindAndApplyShader()
        {
            RenderTarget.Bind();
            DrawBuffer.Bind();
            Shader.Use();
            Texture.Bind();
            Shader.SetUniform("uTexture0", 0);
            Shader.SetUniformVec3("cent1", KMeans[0]);
            Shader.SetUniformVec3("cent2", KMeans[1]);
            Shader.SetUniformVec3("cent3", KMeans[2]);

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
                if (float.IsNaN(KMeans[x][0]) || float.IsNaN(KMeans[x][1]) || float.IsNaN(KMeans[x][2]))
                {
                    KMeans[x] = Vector3D<float>.Zero;
                }
            }
        }

        internal unsafe virtual AvrRenderTarget CreateRenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            return new AvrRenderTarget(gl, Height, Width, Count, internalFormat);
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            base.Init(path, internalFormat, renderTargetSize);
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "centroidCal");
            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, internalFormat);
        }
        public override void Dispose()
        {
            Shader?.Dispose();
            RenderTarget?.Dispose();
            DrawBuffer?.Dispose();
            base.Dispose();
        }
    }

    public class AvgEMGUVideo : EMGUVideo, IKMeansAble, IShaderAble
    {
        public Vector3D<float>[] KMeans { get; set; }
        public SharedResProject.Shader Shader { get; set; }
        public virtual AvrRenderTarget RenderTarget { get; set; }
        public DrawBuffer DrawBuffer { get; set; } = default!;
        public bool IsNaNAbleKMeans { get; set; } = true;

        public AvgEMGUVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
            KMeans = new Vector3D<float>[3] { new Vector3D<float>(0.7f, 0.2f, 0.5f), new Vector3D<float>(1f, 0.5f, 0.7f), new Vector3D<float>(0.5f, 0.7f, 0.2f) };
        }
        public unsafe void BindAndApplyShader()
        {
            RenderTarget.Bind();
            DrawBuffer.Bind();
            Shader.Use();
            Texture.Bind();
            Shader.SetUniform("uTexture0", 0);
            Shader.SetUniformVec3("cent1", KMeans[0]);
            Shader.SetUniformVec3("cent2", KMeans[1]);
            Shader.SetUniformVec3("cent3", KMeans[2]);

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
                if (float.IsNaN(KMeans[x][0]) || float.IsNaN(KMeans[x][1]) || float.IsNaN(KMeans[x][2]))
                {
                    KMeans[x] = Vector3D<float>.Zero;
                }
            }
        }

        internal unsafe virtual AvrRenderTarget CreateRenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            return new AvrRenderTarget(gl, Height, Width, Count, internalFormat);
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            base.Init(path, internalFormat, renderTargetSize);
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "centroidCal");
            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, internalFormat);
        }
        public override void Dispose()
        {
            Shader?.Dispose();
            RenderTarget?.Dispose();
            DrawBuffer?.Dispose();
            base.Dispose();
        }
    }

    public class AvgVLVideo : VLVideo, IKMeansAble, IShaderAble
    {
        public Vector3D<float>[] KMeans { get; set; }
        public SharedResProject.Shader Shader { get; set; }
        public virtual AvrRenderTarget RenderTarget { get; set; }
        public DrawBuffer DrawBuffer { get; set; } = default!;
        public bool IsNaNAbleKMeans { get; set; } = true;

        public AvgVLVideo(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
            KMeans = new Vector3D<float>[3] { new Vector3D<float>(0.7f, 0.2f, 0.5f), new Vector3D<float>(1f, 0.5f, 0.7f), new Vector3D<float>(0.5f, 0.7f, 0.2f) };
        }

        public unsafe void BindAndApplyShader()
        {
            RenderTarget.Bind();
            DrawBuffer.Bind();
            Shader.Use();
            Texture.Bind();
            Shader.SetUniform("uTexture0", 0);
            Shader.SetUniformVec3("cent1", KMeans[0]);
            Shader.SetUniformVec3("cent2", KMeans[1]);
            Shader.SetUniformVec3("cent3", KMeans[2]);

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
                if (float.IsNaN(KMeans[x][0]) || float.IsNaN(KMeans[x][1]) || float.IsNaN(KMeans[x][2]))
                {
                    KMeans[x] = Vector3D<float>.Zero;
                }
            }
        }

        internal unsafe virtual AvrRenderTarget CreateRenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            return new AvrRenderTarget(gl, Height, Width, Count, internalFormat);
        }

        public override void Init(string path, InternalFormat internalFormat, uint renderTargetSize)
        {
            base.Init(path, internalFormat, renderTargetSize);
            DrawBuffer = new DrawBuffer(Gl);
            Shader = new SharedResProject.Shader(Gl, "centroidCal");
            RenderTarget = CreateRenderTarget(Gl, Texture.Height, Texture.Width, renderTargetSize, internalFormat);
        }
        public override void Dispose()
        {
            Shader?.Dispose();
            RenderTarget?.Dispose();
            DrawBuffer?.Dispose();
            base.Dispose();
        }
    }
}
