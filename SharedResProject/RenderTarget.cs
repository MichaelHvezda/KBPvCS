using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KBPvCS
{
    public class RenderTarget
    {
        public uint Handle;
        private GL gl;
        public uint Height;
        public uint Width;
        public uint Count;
        public Texture[] ColorBuffers;
        public GLEnum[] DrawBuffers;
        public unsafe RenderTarget(GL gl, uint Height, uint Width, uint Count)
        {
            this.gl = gl;
            this.Width = Width;
            this.Height = Height;
            this.Count = Count;

            var ss = new float[Height * Width * 4];
            for(int i = 0; i < ss.Length; i++)
            {
                ss[i] = 1f;
            }
            var pixel = new Span<float>(ss);
            this.ColorBuffers = new Texture[Count];
            this.DrawBuffers = new GLEnum[Count];
            for (int i = 0; i < Count; i++)
            {
                this.ColorBuffers[i] = new Texture(gl, pixel, Width, Height);
                this.ColorBuffers[i].Bind();
                gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Nearest);
                gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
                DrawBuffers[i] = GLEnum.ColorAttachment0 + i;
            }

            uint buffer;
            gl.GenFramebuffers(1, &buffer);
            Handle = buffer;
            for (int i = 0; i < Count; i++)
            {
                gl.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
                gl.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.ColorAttachment0 + i, GLEnum.Texture2D, ColorBuffers[i].Handle, 0);
            }

            if (gl.CheckFramebufferStatus(GLEnum.Framebuffer) != GLEnum.FramebufferComplete)
                throw new Exception("velké špatné");
        }

        public void Bind(IWindow window)
        {
            gl.BindFramebuffer(GLEnum.Framebuffer, Handle);
            gl.DrawBuffers(Count, DrawBuffers);
            gl.Viewport(window.Size);
        }
        public void Bind()
        {
            gl.BindFramebuffer(GLEnum.Framebuffer, Handle);
            gl.DrawBuffers(Count, DrawBuffers);
            gl.Viewport(0, 0, Width, Height);
        }
        public void UnBind()
        {
            gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        }
        public void Draw(IWindow window)
        {
            gl.DrawBuffers(Count, DrawBuffers);
            gl.Viewport(window.Size);
        }
        public void Draw()
        {
            gl.Viewport(0, 0, Width, Height);
            gl.DrawBuffers(Count, DrawBuffers);
        }
        public void DrawOnlyBuffers()
        {
            gl.DrawBuffers(Count, DrawBuffers);
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
                var sumVec = this.ColorBuffers[i].avgColor * (int)large;

                kmeansCents[i, 0] = sumVec.X / sumVec.W;
                kmeansCents[i, 1] = sumVec.Y / sumVec.W;
                kmeansCents[i, 2] = sumVec.Z / sumVec.W;
            }
            return kmeansCents;
        }

        public void Dispose()
        {
            foreach (var buf in ColorBuffers)
            {
                buf.Dispose();
            }
            //In order to dispose we need to delete the opengl Handle for the texure.
            gl.DeleteFramebuffer(Handle);
        }
    }
}
