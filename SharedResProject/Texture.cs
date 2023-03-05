using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Silk.NET.Maths;
using SixLabors.ImageSharp.Processing;

namespace KBPvCS
{
    public class Texture : IDisposable
    {
        public uint Handle;
        private GL gl;

        public uint Height;
        public uint Width;
        public int totalMipmapLevels;
        public Vector4D<float> avgColor;
        //public Vector4D<int> avgColorInt;

        public unsafe Texture(GL gl, string path)
        {
            this.gl = gl;

            this.Handle = this.gl.GenTexture();
            Bind();

            //Loading an image using imagesharp.
            using (var img = Image.Load<Rgba32>(path))
            {
                //Reserve enough memory from the gpu for the whole image
                //img.Mutate(p => p.Resize(img.Width / 8, img.Height / 8));
                this.gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
                totalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(img.Width, img.Height))));
                Width = (uint)img.Width;
                Height = (uint)img.Height;
                img.ProcessPixelRows(accessor =>
                {
                    //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        fixed (void* data = accessor.GetRowSpan(y))
                        {
                            //Loading the actual image.
                            this.gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
            }
            SetParameters();
            var pixel = new float[4];
            RecalculateAvrColor();
        }
        public unsafe Texture(GL gl, ImageFrame<Rgba32> img, bool disposeImg = false)
        {
            this.gl = gl;

            this.Handle = this.gl.GenTexture();
            Bind();
            //Loading an image using imagesharp.

            //Reserve enough memory from the gpu for the whole image
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            totalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(img.Width, img.Height))));
            Width = (uint)img.Width;
            Height = (uint)img.Height;
            img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    fixed (void* data = accessor.GetRowSpan(y))
                    {
                        //Loading the actual image.
                        this.gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                    }
                }
            });

            if (disposeImg)
                img.Dispose();

            SetParameters();
            RecalculateAvrColor();
        }
        public unsafe Texture(GL gl, Image<Rgba32> img, bool disposeImg = true)
        {
            this.gl = gl;

            this.Handle = this.gl.GenTexture();
            Bind();

            //Loading an image using imagesharp.

            //Reserve enough memory from the gpu for the whole image
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            totalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(img.Width, img.Height))));
            Width = (uint)img.Width;
            Height = (uint)img.Height;
            img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    fixed (void* data = accessor.GetRowSpan(y))
                    {
                        //Loading the actual image.
                        this.gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                    }
                }
            });

            if (disposeImg)
                img.Dispose();

            SetParameters();
            RecalculateAvrColor();
        }

        public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
        {
            //Saving the gl instance.
            this.gl = gl;
            totalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(width, height))));
            this.Width = width;
            this.Height = height;

            //Generating the opengl Handle;
            Handle = this.gl.GenTexture();
            Bind();

            //We want the ability to create a texture using data generated from code aswell.
            fixed (void* d = &data[0])
            {
                //Setting the data of a texture.
                this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
            }
            SetParameters();
            RecalculateAvrColor();
        }
        public unsafe Texture(GL gl, Span<float> data, uint width, uint height)
        {
            //Saving the gl instance.
            this.gl = gl;
            totalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(width, height))));
            this.Width = width;
            this.Height = height;

            //Generating the opengl Handle;
            Handle = this.gl.GenTexture();
            Bind();

            //We want the ability to create a texture using data generated from code aswell.
            fixed (void* d = &data[0])
            {
                //Setting the data of a texture.
                //this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
                this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
            }
            SetParameters();
            RecalculateAvrColor();
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            //When we bind a texture we can choose which textureslot we can bind it to.
            gl.ActiveTexture(textureSlot);
            gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            //In order to dispose we need to delete the opengl Handle for the texure.
            gl.DeleteTexture(Handle);
        }
        public unsafe void RecalculateAvrColor()
        {
            Bind();
            gl.GenerateMipmap(TextureTarget.Texture2D);
            var pixel = new float[4];
            fixed (void* p = &pixel[0])
            {
                gl.GetTexImage(TextureTarget.Texture2D, totalMipmapLevels - 1, PixelFormat.Rgba, PixelType.Float, p);
                avgColor = new Vector4D<float>() { X = pixel[0], Y = pixel[1], Z = pixel[2], W = pixel[3] };
                //float valFloat = pixel[0];
                //uint val = *((uint*)&valFloat);
                //avgColorInt = new Vector4D<int>
                //{
                //    X = (int)(val & 0xFF),
                //    Y = (int)((val >> 8) & 0xFF),
                //    Z = (int)((val >> 16) & 0xFF),
                //    W = (int)((val >> 24) & 0xFF)
                //};
            }
        }
        private void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 12);
            //Generating mipmaps.
            gl.GenerateMipmap(TextureTarget.Texture2D);

        }
    }
}
