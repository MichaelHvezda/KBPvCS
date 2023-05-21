using SharedProject.Interface;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Base
{
    public class BaseRenderTarget<TTexture> : BaseGLClass, IRenderTarget
        where TTexture : BaseGLClass, ITexture
    {
        public uint Handle { get; set; }
        public uint Height { get; set; }
        public uint Width { get; set; }
        public uint Count { get; set; }
        public ITexture[] ColorBuffers { get; set; } = Array.Empty<TTexture>();
        public GLEnum[] DrawBuffers { get; set; } = Array.Empty<GLEnum>();

        public unsafe BaseRenderTarget(GL gl, uint Height, uint Width, uint Count,InternalFormat internalFormat) : base(gl)
        {
            this.Init(Height, Width, Count, internalFormat);
        }

        public virtual unsafe void Init(uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            this.Width = Width;
            this.Height = Height;
            this.Count = Count;

            var ss = new float[Height * Width * 4];
            for (int i = 0; i < ss.Length; i++)
            {
                ss[i] = 1f;
            }
            var pixel = new Span<float>(ss);
            this.ColorBuffers = new TTexture[Count];
            this.DrawBuffers = new GLEnum[Count];
            for (int i = 0; i < Count; i++)
            {
                this.ColorBuffers[i] = TTexture.Init(Gl,pixel, Width, Height, internalFormat);
                this.ColorBuffers[i].Bind();
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
                DrawBuffers[i] = GLEnum.ColorAttachment0 + i;
            }

            uint buffer;
            Gl.GenFramebuffers(1, &buffer);
            Handle = buffer;
            for (int i = 0; i < Count; i++)
            {
                Gl.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
                Gl.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.ColorAttachment0 + i, GLEnum.Texture2D, ColorBuffers[i].Handle, 0);
            }

            if (Gl.CheckFramebufferStatus(GLEnum.Framebuffer) != GLEnum.FramebufferComplete)
                throw new Exception("velké špatné");
        }

        public static IRenderTarget Init(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            return new BaseRenderTarget<TTexture>(gl, Height, Width, Count, internalFormat);
        }

        public void Bind(IWindow window)
        {
            Gl.BindFramebuffer(GLEnum.Framebuffer, Handle);
            Gl.DrawBuffers(Count, DrawBuffers);
            Gl.Viewport(window.Size);
        }
        public void Bind()
        {
            Gl.BindFramebuffer(GLEnum.Framebuffer, Handle);
            Gl.DrawBuffers(Count, DrawBuffers);
            Gl.Viewport(0, 0, Width, Height);
        }
        public void UnBind()
        {
            Gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        }
        public void Draw(IWindow window)
        {
            Gl.DrawBuffers(Count, DrawBuffers);
            Gl.Viewport(window.Size);
        }
        public void Draw()
        {
            Gl.Viewport(0, 0, Width, Height);
            Gl.DrawBuffers(Count, DrawBuffers);
        }
        public void DrawOnlyBuffers()
        {
            Gl.DrawBuffers(Count, DrawBuffers);
        }
        public float[,] RecalculateAndGetAvrColor()
        {
            float[,] kmeansCents = new float[Count, 3];
            for (int i = 0; i < Count; i++)
            {
                this.ColorBuffers[i].RecalculateAvrColor();
                //gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
                //gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
                var large = this.ColorBuffers[i].Width * this.ColorBuffers[i].Height;
                var sumVec = this.ColorBuffers[i].AvgColor * (int)large;

                kmeansCents[i, 0] = sumVec.X / sumVec.W;
                kmeansCents[i, 1] = sumVec.Y / sumVec.W;
                kmeansCents[i, 2] = sumVec.Z / sumVec.W;
            }
            return kmeansCents;
        }

        public override void Dispose()
        {
            foreach (var buf in ColorBuffers)
            {
                buf?.Dispose();
            }
            //In order to dispose we need to delete the opengl Handle for the texure.
            Gl.DeleteFramebuffer(Handle);

            base.Dispose();
        }
    }
}
