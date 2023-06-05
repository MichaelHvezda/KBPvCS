using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Collections.Specialized.BitVector32;

namespace SharedProject.Base
{
    public abstract class BaseTexture : BaseGLClass, ITexture
    {
        public uint Handle { get; set; }

        public uint Height { get; set; }
        public uint Width { get; set; }
        public int TotalMipmapLevels { get; set; }
        public Vector4D<float> AvgColor { get; set; } = new();
        public PixelFormat PixelFormat { get; set; }
        public InternalFormat InternalFormat { get; set; }

        public unsafe BaseTexture(GL gl, string path, InternalFormat internalFormat) : base(gl)
        {
            //Loading an image using imagesharp.
            using (var img = Image.Load<Rgba32>(path))
            {

                Action ss = () => img.ProcessPixelRows(accessor =>
                {
                    //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        fixed (void* data = accessor.GetRowSpan(y))
                        {
                            //Loading the actual image.
                            this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
                CreateMain(null, (uint)img.Width, (uint)img.Height, internalFormat, action: ss);
            }
        }

        public unsafe BaseTexture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl)
        {
            //Loading an image using imagesharp.
            Action ss = () => img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    fixed (void* data = accessor.GetRowSpan(y))
                    {
                        //Loading the actual image.
                        this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                    }
                }
            });
            CreateMain(null, (uint)img.Width, (uint)img.Height, internalFormat, action: ss);

            //Reserve enough memory from the gpu for the whole image

            if (disposeImg)
                img.Dispose();

        }

        public unsafe BaseTexture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null!) : base(gl)
        {
            //We want the ability to create a texture using data generated from code aswell.

            //Setting the data of a texture.
            //this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);

            CreateMain(data, width, height, internalFormat, pixelFormat, action);
        }

        public virtual unsafe void CreateMain(void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null!)
        {
            //Generating the opengl Handle;
            Handle = this.Gl.GenTexture();
            Bind();
            this.Width = width;
            this.Height = height;
            this.PixelFormat = pixelFormat;
            this.InternalFormat = InternalFormat;
            this.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, width, height, 0, pixelFormat, PixelType.UnsignedByte, data);

            action?.Invoke();
            SetParameters();
        }
        public unsafe void ChangeContent(void* data)
        {
            Bind();
            this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, PixelFormat, PixelType.UnsignedByte, data);
        }

        public unsafe void ChangeContent(ImageFrame<Rgba32> img)
        {
            Bind();
            img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    fixed (void* data = accessor.GetRowSpan(y))
                    {
                        //Loading the actual image.
                        this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                    }
                }
            }
            );
        }

        public void Bind(TextureUnit textureSlot)
        {
            //When we bind a texture we can choose which textureslot we can bind it to.
            Gl.ActiveTexture(textureSlot);
            Gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public override void Dispose()
        {
            //In order to dispose we need to delete the opengl Handle for the texure.
            Gl.DeleteTexture(Handle);
            base.Dispose();
        }

        public void Bind()
        {
            //When we bind a texture we can choose which textureslot we can bind it to.
            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void UnBind()
        {
            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, 0);
        }
        internal abstract void CalculateTotalMipmapLevels();
        public abstract void RecalculateAvrColor();
        internal abstract void SetParameters();
    }
}
