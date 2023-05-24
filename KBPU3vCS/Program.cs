// See https://aka.ms/new-console-template for more information
using SharedProject.Base;
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

namespace KBPU21vCS;
class Program
{
    private static IWindow window;
    private static GL Gl;

    //Our new abstracted objects, here we specify what the types are.

    private static SharedResProject.Shader Shader;
    private static DrawBuffer DrawBufferr;
    private static BaseTexture Texture;
    private static Video Video;

    public static int FramePosition { get; set; } = 0;
    public static int ImagePosition { get; set; } = 0;
    public static bool VideoStop { get; set; }

    public static DateTime DateNow { get; set; }


    private static readonly float GreenH = 95 / 360f;
    private static readonly float BlueH = 200 / 360f;

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
        Texture = new(Gl, ResourcesProvider.Back, InternalFormat.Rgba16f);
        Video = new(Gl, ResourcesProvider.Video3, InternalFormat.Rgba16f, 4);
        Video.KMeans = new float[4, 3] { { 0.7f, 0.2f, 0.5f }, { 1f, 0.5f, 0.7f }, { 0.4f, 0.7f, 0.2f }, { BlueH, 0.0f, 0.0f } };

        Console.WriteLine("res loaded");
        DateNow = DateTime.Now;
    }
    private static unsafe void OnRender(double obj)
    {
        Gl.Clear(ClearBufferMask.ColorBufferBit);

        DrawBufferr.Bind();

        Shader.Use();
        //Bind a texture and and set the uTexture0 to use texture0.

        Video.Texture.Bind(TextureUnit.Texture0);
        Shader.SetUniform("uTexture0", 0);

        Texture.Bind(TextureUnit.Texture1);
        Shader.SetUniform("uTexture1", 1);

        //Video.RenderTarget.ColorBuffers[ImagePosition].Bind(TextureUnit.Texture2);
        //Shader.SetUniform("uTexture2", 2);

        Video.RenderTarget.ColorBuffers[3].Bind(TextureUnit.Texture2);
        Shader.SetUniform("uTexture2", 2);

        Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);

        if (!VideoStop)
        {
            for (var x = 0; x < 4; x++)
            {
                Console.Write("{");
                for (var y = 0; y < 3; y++)
                {
                    Console.Write(Video.KMeans[x, y] + " ");
                }

                Console.Write("}, ");
            }
            Console.WriteLine();

            Video.NextFrame();
            Video.BindAndApplyShader();
            Console.WriteLine(Video.FramePosition);

        }
        if (Video.FramePosition == 0)
        {
            var fps = Video.FrameCount / (decimal)(DateTime.Now - DateNow).Seconds;
            Console.WriteLine($"{fps} fps");
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