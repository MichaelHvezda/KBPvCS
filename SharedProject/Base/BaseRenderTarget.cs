using SharedProject.Implementation;
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
    public abstract class BaseRenderTarget : BaseGLClass, IRenderTarget
    {
        public uint Handle { get; set; }
        public uint Height { get; set; }
        public uint Width { get; set; }
        public uint Count { get; set; }
        public ITexture[] ColorBuffers { get; set; } = Array.Empty<ITexture>();
        public GLEnum[] DrawBuffers { get; set; } = Array.Empty<GLEnum>();

        public unsafe BaseRenderTarget(GL gl, uint Height, uint Width, uint Count, InternalFormat internalFormat) : base(gl)
        {
            this.Init(Height, Width, Count, internalFormat);
        }

        public virtual unsafe void Init(uint Height, uint Width, uint Count, InternalFormat internalFormat)
        {
            this.Width = Width;
            this.Height = Height;
            this.Count = Count;

            var pixel = new byte[Height * Width * 4];
            this.ColorBuffers = new ITexture[Count];
            this.DrawBuffers = new GLEnum[Count];
            fixed (void* p = &pixel[0])
            {
                for (int i = 0; i < Count; i++)
                {
                    this.ColorBuffers[i] = CreateTexture(Gl, p, Width, Height, internalFormat);
                    this.ColorBuffers[i].Bind();
                    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
                    Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
                    DrawBuffers[i] = GLEnum.ColorAttachment0 + i;
                }
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

        internal unsafe abstract ITexture CreateTexture(GL gl, void* pixel, uint width, uint height, InternalFormat internalFormat);


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
