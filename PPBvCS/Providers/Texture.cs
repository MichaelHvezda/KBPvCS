using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBvCS.Providers
{
    public sealed class Texture : SharedProject.Implementation.Texture
    {
        public Texture(GL gl, string path, InternalFormat internalFormat) : base(gl, path, internalFormat)
        {
        }

        public Texture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl, img, internalFormat, disposeImg)
        {
        }

        public unsafe Texture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null) : base(gl, data, width, height, internalFormat, pixelFormat, action)
        {
        }
        //protected override void SetParameters()
        //{
        //    //Setting some texture perameters so the texture behaves as expected.
        //    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        //    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
        //    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
        //    //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.NearestMipmapNearest);
        //    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
        //    //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        //    //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 16);
        //}

        protected override void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.NearestMipmapNearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 16);
        }
    }
}
