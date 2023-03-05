// See https://aka.ms/new-console-template for more information
using SharedResProject;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace KBPvCS
{
    class Program
    {
        private static IWindow window;
        private static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        private static SharedResProject.Shader Shader;
        private static DrawBuffer DrawBufferr;
        private static SharedResProject.Texture Texture;
        private static Video Video;
        private static RenderTarget RenderTarget;

        public static int FramePosition { get; set; } = 0;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Silk.NET.Windowing.Window.Create(options);
            window.Load += OnLoad;
            window.Render += OnRender;
            window.Resize += OnResize;
            window.Closing += OnClose;

            window.Run();
        }

        private static void OnLoad()
        {
            IInputContext input = window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }

            Gl = GL.GetApi(window);

            //Instantiating our new abstractions
            DrawBufferr = new(Gl);
            Shader = new(Gl, "kmean");
            Texture = new(Gl, ResourcesProvider.Big);
            Video = new(Gl, ResourcesProvider.Video1);
            //RenderTarget = new RenderTarget(Gl, Video.Texture.Height, Video.Texture.Width, 1);
            Console.WriteLine("res loaded");
        }
        private static unsafe void OnRender(double obj)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            //RenderTarget.Bind();
            DrawBufferr.Bind();
            Shader.Use();
            //Bind a texture and and set the uTexture0 to use texture0.
            Video.Texture.Bind(TextureUnit.Texture0);
            Shader.SetUniform("uTexture0", 0);

            Texture.Bind(TextureUnit.Texture1);
            Shader.SetUniform("uTexture1", 1);

            Video.RenderTarget.ColorBuffers[2].Bind(TextureUnit.Texture2);
            Shader.SetUniform("uTexture2", 2);

            //Gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
            //Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);

            Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);


            //Video.RenderTarget.UnBind();
            //RenderTarget.UnBind();
            //RenderTarget.ColorBuffers[0].Bind();
            //Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);


            //RenderTarget.ColorBuffers[0].Bind();
            //Gl.BindTexture(GLEnum.Texture2D, Video.Texture.Handle);
            ////Gl.ReadnPixels(0,0, Video.Texture.Width , Video.Texture.Height, GLEnum.Rgba,GLEnum.UnsignedByte,(uint)pixel.Length, p);
            //Gl.GetTexImage(GLEnum.Texture2D, 0, GLEnum.Rgba, GLEnum.Float, pixel);
            //void* data_ptr = Gl.MapBuffer(GLEnum.PixelPackBuffer, GLEnum.ReadOnly);
            //var pixelc = new float[9];
            //fixed (void* p = &pixelc[0])
            //{

            //    Gl.GenerateTextureMipmap(RenderTarget.ColorBuffers[0].Handle);
            //    Gl.Flush();
            //    Gl.GetTextureImage(RenderTarget.ColorBuffers[0].Handle,0, GLEnum.Rgba, GLEnum.UnsignedByte,4, p);
            //    var avgColor = new Vector4D<float>() { X = pixelc[0], Y = pixelc[1], Z = pixelc[2], W = pixelc[3] };
            //    float valFloat = pixelc[0];
            //    uint val = *((uint*)&valFloat);
            //    var avgColorFloat = new Vector4D<int>
            //    {
            //        X = (int)(val & 0xFF),
            //        Y = (int)((val >> 8) & 0xFF),
            //        Z = (int)((val >> 16) & 0xFF),
            //        W = (int)((val >> 24) & 0xFF)
            //    };
            //}

            //var pixel = new byte[Video.RenderTarget.ColorBuffers[1].Width * Video.RenderTarget.ColorBuffers[1].Height * 4];
            //fixed (void* p = &pixel[0])
            //{
            //    //Gl.ReadnPixels(0, 0, 10, 10, GLEnum.Rgba, GLEnum.UnsignedByte, (uint)pixel.Length, p);
            //    Gl.GetTextureImage(Video.RenderTarget.ColorBuffers[0].Handle, 0, GLEnum.Rgba, GLEnum.Byte, Video.RenderTarget.ColorBuffers[1].Width * Video.RenderTarget.ColorBuffers[1].Height * 4, p);

            //    //float valFloat = pixel.FirstOrDefault(p => p != 0);
            //    //uint val = *((uint*)&valFloat);
            //    //var sd = new Vector4D<int>
            //    //{
            //    //    X = (int)(val & 0xFF),
            //    //    Y = (int)((val >> 8) & 0xFF),
            //    //    Z = (int)((val >> 16) & 0xFF),
            //    //    W = (int)((val >> 24) & 0xFF)
            //    //};

            //    var img = Image.LoadPixelData<Rgba32>(pixel, (int)Video.RenderTarget.ColorBuffers[1].Width, (int)Video.RenderTarget.ColorBuffers[1].Height);
            //    img.SaveAsPngAsync("C:\\Users\\Hvězdič\\source\\repos\\KBPvCS\\KBPvCS\\resources\\img1.png");
            //}
            //Gl.UnmapBuffer(GLEnum.PixelPackBuffer);
            foreach (var i in Video.KMeans)
            {

                Console.Write(i + " ");
            }

            Console.WriteLine();
            Video.NextFrame();
            Video.BindAndApplyShader();
            Console.WriteLine(Video.FramePosition);
        }

        private static unsafe void OnResize(Vector2D<int> obj)
        {
            window.Size = obj;
        }

        private static void OnClose()
        {
            //Remember to dispose all the instances.
            DrawBufferr.Dispose();
            Shader.Dispose();
            Texture.Dispose();
            Video.Dispose();
            //RenderTarget.Dispose();
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Escape)
            {
                window.Close();
            }
            if (arg2 == Key.Right)
            {
                FramePosition++;
            }
            if (arg2 == Key.Left)
            {
                FramePosition--;
            }
        }
    }
}