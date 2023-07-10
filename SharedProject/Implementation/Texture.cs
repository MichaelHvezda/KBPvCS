using SharedProject.Base;
using SharedProject.Interface;
using SharedProject.Interface.Atomic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Implementation
{
    public class Texture : BaseTexture
    {
        public Texture(GL gl, string path, InternalFormat internalFormat) : base(gl, path, internalFormat)
        {
        }

        public Texture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl, img, internalFormat, disposeImg)
        {
        }

        public unsafe Texture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null!) : base(gl, data, width, height, internalFormat, pixelFormat, action)
        {
        }

        public override void RecalculateAvrColor()
        {
            throw new NotImplementedException($"For this try {nameof(AvgTexture)} class");
        }

        protected override void CalculateTotalMipmapLevels()
        {
            throw new NotImplementedException($"For this try {nameof(AvgTexture)} class");
        }

        protected override void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.NearestMipmapNearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 16);
        }
    }

    public sealed class AvgTexture : Texture
    {
        public AvgTexture(GL gl, string path, InternalFormat internalFormat) : base(gl, path, internalFormat)
        {
        }

        public AvgTexture(GL gl, ImageFrame<Rgba32> img, InternalFormat internalFormat, bool disposeImg = false) : base(gl, img, internalFormat, disposeImg)
        {
        }

        public unsafe AvgTexture(GL gl, void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null!) : base(gl, data, width, height, internalFormat, pixelFormat, action)
        {
        }

        protected override void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            //Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.NearestMipmapNearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
        }

        protected override void CalculateTotalMipmapLevels()
        {
            this.TotalMipmapLevels = (int)(1 + Math.Floor(Math.Log2(Math.Max(Width, Height))));
        }

        public override unsafe void CreateMain(void* data, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat = PixelFormat.Rgba, Action action = null)
        {
            Handle = this.Gl.GenTexture();
            Bind();
            this.Width = width;
            this.Height = height;
            this.PixelFormat = pixelFormat;
            this.InternalFormat = internalFormat;
            this.CalculateTotalMipmapLevels();
            this.Gl.TexStorage2D(TextureTarget.Texture2D, (uint)TotalMipmapLevels, (GLEnum)internalFormat, width, height);
            if (data is not null)
            {
                this.Gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, pixelFormat, PixelType.UnsignedByte, data);
            }

            action?.Invoke();

            SetParameters();
            this.RecalculateAvrColor();
        }
        public override unsafe void RecalculateAvrColor()
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
    }
}
