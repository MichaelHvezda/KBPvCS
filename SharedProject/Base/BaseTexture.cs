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

namespace SharedProject.Base
{
    public class BaseTexture : BaseGLClass, ITexture
    {
        public uint Handle { get; set; }

        public uint Height { get; set; }
        public uint Width { get; set; }
        public int TotalMipmapLevels { get; set; }
        public Vector4D<float> AvgColor { get; set; } = new();

        public unsafe BaseTexture(GL gl, string path, InternalFormat internalFormat) : base(gl)
        {
            this.Handle = this.Gl.GenTexture();
            Bind();

            //Loading an image using imagesharp.
            using (var img = Image.Load<Rgba32>(path))
            {
                //Reserve enough memory from the gpu for the whole image
                //img.Mutate(p => p.Resize(img.Width / 8, img.Height / 8));
                this.Gl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);

                Width = (uint)img.Width;
                Height = (uint)img.Height;
                this.CalculateTotalMipmapLevels();
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
                });
            }
            SetParameters();
            RecalculateAvrColor();
        }

        public unsafe BaseTexture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl)
        {
            this.Handle = this.Gl.GenTexture();
            Bind();
            //Loading an image using imagesharp.

            //Reserve enough memory from the gpu for the whole image
            this.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            Width = (uint)img.Width;
            Height = (uint)img.Height;
            this.CalculateTotalMipmapLevels();
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
            });

            if (disposeImg)
                img.Dispose();

            SetParameters();
            RecalculateAvrColor();
        }

        public unsafe BaseTexture(GL gl, Span<float> data, uint width, uint height, InternalFormat internalFormat) : base(gl)
        {
            //Saving the gl instance.
            this.Width = width;
            this.Height = height;

            this.CalculateTotalMipmapLevels();
            //Generating the opengl Handle;
            Handle = this.Gl.GenTexture();
            Bind();

            //We want the ability to create a texture using data generated from code aswell.
            fixed (void* d = &data[0])
            {
                //Setting the data of a texture.
                //this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
                this.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
            }
            SetParameters();
            RecalculateAvrColor();
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
        public unsafe void RecalculateAvrColor()
        {
            Bind();
            Gl.GenerateMipmap(TextureTarget.Texture2D);
            var pixel = new float[4];
            fixed (void* p = &pixel[0])
            {
                Gl.GetTexImage(TextureTarget.Texture2D, TotalMipmapLevels - 1, PixelFormat.Rgba, PixelType.Float, p);
                AvgColor = new Vector4D<float>() { X = pixel[0], Y = pixel[1], Z = pixel[2], W = pixel[3] };

            }
        }
        private void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 12);
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

        private void CalculateTotalMipmapLevels()
        {
            this.TotalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(Width, Height))));
        }

        public static ITexture Init(GL gl, Span<float> data, uint width, uint height, InternalFormat internalFormat)
        {
            return new BaseTexture(gl, data, width, height, internalFormat);
        }

        public static ITexture Init(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false)
        {
            return new BaseTexture(gl, img, internalFormat, disposeImg);
        }
    }
}
