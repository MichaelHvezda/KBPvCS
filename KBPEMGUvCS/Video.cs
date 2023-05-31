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
using Emgu.CV;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace KBPEMGUvCS
{
    public sealed class Video : BaseGLClass
    {
        //public Image<Rgba32> video { get; } = default!;
        private VideoCapture video { get; } = default!;
        public InternalFormat InternalFormat { get; } = default!;

        public Texture Texture { get; set; } = default!;

        public int FrameCount { get; set; }
        public int FramePosition { get; set; } = 0;
        public Video(GL gl, string path, InternalFormat internalFormat) : base(gl)
        {
            video = new VideoCapture(path);
            InternalFormat = internalFormat;
            LoadFrame();
            //video = Image.Load<Rgba32>(VideoConfiguration.GetConfiguration(), path);

            //FrameCount = video.Frames.Count;

        }
        private unsafe void LoadFrame()
        {
            //var time = DateTime.Now;
            var mat = new Mat();
            if (video.Read(mat))
            {
                if (Texture is null)
                {
                    fixed (void* d = &mat.GetRawData()[0])
                    {

                        //Console.WriteLine("texture cscd {0}", (time - DateTime.Now).TotalMilliseconds);
                        Texture = new Texture(Gl, d, (uint)mat.Width, (uint)mat.Height, InternalFormat);
                    }
                }

                //Console.WriteLine("texture readed {0}", (time - DateTime.Now).TotalMilliseconds);


                fixed (void* d = &mat.GetRawData()[0])
                {
                    Texture.ChangeContent(d);
                }
            }
            //Console.WriteLine("texture creation {0}", (time - DateTime.Now).TotalMilliseconds);
        }

        public void NextFrame()
        {
            //var time = DateTime.Now;
            FramePosition++;
            //if (FrameCount <= FramePosition)
            //    FramePosition = 0;

            LoadFrame();
            //Console.WriteLine("texture creation {0}", (time - DateTime.Now).TotalMilliseconds);
        }
        public void GetFrame(int position)
        {
            if (FrameCount <= position || position < 0)
                position = 0;

            //var img = video.Frames[position];

            //if (img.PixelBuffer is not null)
            //{
            //    if (Texture is not null)
            //    {
            //        Texture.Dispose();
            //    }

            //    Texture = TTexture.Init(Gl, img, InternalFormat.Rgba8);
            //}
        }
        public override void Dispose()
        {
            video?.Dispose();
            Texture?.Dispose();

            base.Dispose();
        }

        public static Video Init(GL gl, string path, InternalFormat internalFormat)
        {
            return new Video(gl, path, internalFormat);
        }
    }
}
