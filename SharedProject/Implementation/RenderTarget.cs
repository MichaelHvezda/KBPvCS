using FFmpeg.AutoGen;
using SharedProject.Base;
using SharedProject.Interface;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Implementation
{
    public class RenderTarget : BaseRenderTarget
    {
        public RenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat) : base(gl, Height, Width, Count, internalFormat)
        {
        }

        internal unsafe override ITexture CreateTexture(GL gl, void* pixel, uint width, uint height, InternalFormat internalFormat)
        {
            return new Texture(gl, pixel, width, height, internalFormat);
        }
    }

    public class AvrRenderTarget : RenderTarget, IAvrRenderTarget
    {
        public AvrRenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat) : base(gl, Height, Width, Count, internalFormat)
        {
        }
        public Vector3D<float>[] RecalculateAndGetAvrColor()
        {
            Vector3D<float>[] kmeansCents = new Vector3D<float>[Count];
            for (int i = 0; i < Count; i++)
            {
                this.ColorBuffers[i].RecalculateAvrColor();
                //gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
                //gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
                var large = this.ColorBuffers[i].Width * this.ColorBuffers[i].Height;
                var sumVec = this.ColorBuffers[i].AvgColor * (int)large;

                kmeansCents[i] = new Vector3D<float>(sumVec.X / sumVec.W, sumVec.Y / sumVec.W, sumVec.Z / sumVec.W);
            }
            return kmeansCents;
        }
    }
}
