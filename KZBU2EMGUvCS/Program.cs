// See https://aka.ms/new-console-template for more information
using SharedProject.Base;
using SharedProject.Implementation;
using SharedProject.Interface;
using SharedResProject;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Data;

namespace KZBU2EMGUvCS;
class Program
{
    private static IWindow window;
    private static GL Gl;

    //Our new abstracted objects, here we specify what the types are.

    private static SharedResProject.Shader Shader;
    private static DrawBuffer DrawBufferr;
    private static ITexture Texture;
    private static Video Video;

    public static int FramePosition { get; set; } = 0;
    public static int ImagePosition { get; set; } = 0;
    public static bool VideoStop { get; set; }

    public static DateTime DateNow { get; set; }


    private static readonly float GreenH = 95 / 360f;
    private static readonly float BlueH = 220 / 360f;

    private static void Main(string[] args)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "LearnOpenGL with Silk.NET";
        options.VSync = false;
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
        Texture = new SharedProject.Implementation.Texture(Gl, ResourcesProvider.Back, InternalFormat.Rgba8);
        Video = new Video(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba4, 3);

        Console.WriteLine("res loaded");
        DateNow = DateTime.Now;
    }
    private static unsafe void OnRender(double obj)
    {
        //var time = DateTime.Now;
        Gl.Clear(ClearBufferMask.ColorBufferBit);

        DrawBufferr.Bind();

        Shader.Use();
        //Bind a texture and and set the uTexture0 to use texture0.

        Gl.Viewport(window.Size);
        Video.Texture.Bind(TextureUnit.Texture0);
        Shader.SetUniform("uTexture0", 0);

        Texture.Bind(TextureUnit.Texture1);
        Shader.SetUniform("uTexture1", 1);

        //Video.RenderTarget.ColorBuffers[ImagePosition].Bind(TextureUnit.Texture2);
        //Shader.SetUniform("uTexture2", 2);
        Video.RenderTarget.ColorBuffers[Video.GetBGTextureId(BlueH)].Bind(TextureUnit.Texture2);
        Shader.SetUniform("uTexture2", 2);

        Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);

        if (!VideoStop)
        {
            //for (var x = 0; x < 3; x++)
            //{
            //    Console.Write("{");
            //    Console.Write(Video.KMeans[x] + " ");

            //    Console.Write("}, ");
            //}
            //Console.WriteLine();

            //Console.WriteLine("render {0}", (time - DateTime.Now).TotalMilliseconds);
            Video.NextFrame();
            //Console.WriteLine("next {0}", (time - DateTime.Now).TotalMilliseconds);
            //var time = DateTime.Now;
            Video.BindAndApplyShader();
            //Console.WriteLine("shader {0}", (time - DateTime.Now).TotalMilliseconds);
            //Console.WriteLine("shader {0}", (time - DateTime.Now).TotalMilliseconds);
            //Console.WriteLine(Video.FramePosition);
            //Console.WriteLine("BG image ID " + Video.GetBGTextureId(BlueH));

        }
        if (Video.FramePosition == 0)
        {
            var fps = (double)396 / (double)((DateTime.Now - DateNow).TotalMilliseconds / 1000d);
            Console.WriteLine("{0} fps {1} time", fps, (decimal)(DateTime.Now - DateNow).TotalMilliseconds);
            DateNow = DateTime.Now;
        }
    }

    private static unsafe void OnResize(Vector2D<int> obj)
    {
        window.Size = obj;
    }

    private static void OnClose()
    {
        //Remember to dispose all the instances.
        DrawBufferr?.Dispose();
        Shader?.Dispose();
        Texture?.Dispose();
        Video?.Dispose();
        Gl?.Dispose();
    }

    private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        if (arg2 == Key.Escape)
        {
            window.Close();
        }
        if (arg2 == Key.Right)
        {
            ImagePosition++;
            ImagePosition = (ImagePosition + 3) % 3;
        }
        if (arg2 == Key.Left)
        {
            ImagePosition--;
            ImagePosition = (ImagePosition + 3) % 3;
        }
        if (arg2 == Key.Space)
        {
            Video.IsNaNAbleKMeans = !Video.IsNaNAbleKMeans;
        }
        if (arg2 == Key.S)
        {
            VideoStop = !VideoStop;
        }
        if (arg2 == Key.N)
        {
            Video.NextFrame();
            Video.BindAndApplyShader();
        }
    }
}