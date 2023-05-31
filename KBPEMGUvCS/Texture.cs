using SharedProject.Base;
using SharedProject.Interface;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Emgu.CV;

namespace KBPEMGUvCS
{
    public sealed class Texture : BaseGLClass
    {
        public uint Handle { get; set; }

        public uint Height { get; set; }
        public uint Width { get; set; }
        public int TotalMipmapLevels { get; set; }
        public Vector4D<float> AvgColor { get; set; } = new();

        public unsafe Texture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat) : base(gl)
        {
            //Saving the gl instance.
            this.Width = width;
            this.Height = height;

            //this.CalculateTotalMipmapLevels();
            //Generating the opengl Handle;
            Handle = this.Gl.GenTexture();
            Bind();

            //We want the ability to create a texture using data generated from code aswell.


            //this.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, width, height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, null);
            //this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Bgr, PixelType.UnsignedByte, data);


            //Setting the data of a texture.
            this.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, width, height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, data);
            //fixed (void* d = &data[0])
            //{
            //    //Setting the data of a texture.
            //    //this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
            //    this.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)internalFormat, width, height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, d);
            //}
            SetParameters();
            //RecalculateAvrColor();
        }
        public unsafe void ChangeContent(void* data)
        {
            Bind();
            this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, PixelFormat.Bgr, PixelType.UnsignedByte, data);
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
            //var pixel = new float[4];
            //fixed (void* p = &pixel[0])
            //{
            //    Gl.GetTexImage(TextureTarget.Texture2D, TotalMipmapLevels - 1, PixelFormat.Rgba, PixelType.Float, p);
            //    AvgColor = new Vector4D<float>() { X = pixel[0], Y = pixel[1], Z = pixel[2], W = pixel[3] };

            //}
        }
        private void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 16);
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
    }
}
