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
using SharedProject.Base;
using Image = SixLabors.ImageSharp.Image;

namespace KBPUsWFvCS
{
    public class Video<TTexture, TRenderTarget> : BaseGLClass, IVideo
        where TRenderTarget : BaseGLClass, IRenderTarget
        where TTexture : BaseGLClass, ITexture
    {
        public Image<Rgba32> video { get; } = default!;

        public ITexture Texture { get; set; } = default!;

        public int FrameCount { get; set; }
        public int FramePosition { get; set; } = 0;
        public Video(GL gl, string path, InternalFormat internalFormat) : base(gl)
        {
            video = Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);
            FrameCount = video.Frames.Count;

            Texture = TTexture.Init(Gl, video.Frames[FramePosition], internalFormat);
        }

        public void NextFrame()
        {
            //var time = DateTime.Now;
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

                Texture = TTexture.Init(Gl, img, InternalFormat.Rgba8);
            }
            //Console.WriteLine("texture creation {0}", (time - DateTime.Now).TotalMilliseconds);
        }
        public void GetFrame(int position)
        {
            if (FrameCount <= position || position < 0)
                position = 0;

            var img = video.Frames[position];

            if (img.PixelBuffer is not null)
            {
                if (Texture is not null)
                {
                    Texture.Dispose();
                }

                Texture = TTexture.Init(Gl, img, InternalFormat.Rgba8);
            }
        }
        public override void Dispose()
        {
            video?.Dispose();
            Texture?.Dispose();

            base.Dispose();
        }

        public static new IVideo Init(GL gl, string path, InternalFormat internalFormat)
        {
            return new Video<TTexture, TRenderTarget>(gl, path, internalFormat);
        }
    }
    public class Video : Video<BaseTexture, BaseRenderTarget<BaseTexture>>
    {
        public Video(GL gl, string path, InternalFormat internalFormat) : base(gl, path, internalFormat)
        {
        }

        public static new IVideo Init(GL gl, string path, InternalFormat internalFormat)
        {
            return new Video(gl, path, internalFormat);
        }
    }
}
