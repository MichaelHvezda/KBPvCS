using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.Devices;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Silk.NET.Windowing;
using SharedProject.Base;
using SharedProject.Interface;
using SharedResProject;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using SharedProject.Implementation;

namespace KBPUsWFvCS;

static class Program
{
    private static IWindow window;
    private static GL Gl;

    //Our new abstracted objects, here we specify what the types are.

    private static SharedResProject.Shader Shader;
    private static DrawBuffer DrawBufferr;
    private static SharedProject.Implementation.Texture Texture;
    private static SLVideo Video;
    private static Form FormSetting;
    public static bool VideoStop { get; set; }

    public static DateTime DateNow { get; set; }



    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "LearnOpenGL with Silk.NET";
        options.VSync = false;
        window = Silk.NET.Windowing.Window.Create(options);
        window.Load += OnLoad;
        window.Update += OnUpdate;
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
        Video = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);

        FormSetting = new Form1();
        FormSetting.Show();

        Console.WriteLine("res loaded");
        DateNow = DateTime.Now;
    }
    private static unsafe void OnUpdate(double obj)
    {
        if (FormSetting.IsDisposed)
        {
            window.Close();
        }
        FormSetting.Update();
    }
    private static unsafe void OnRender(double obj)
    {
        //var time = DateTime.Now;
        Gl.Clear(ClearBufferMask.ColorBufferBit);

        DrawBufferr.Bind();

        Shader.Use();

        Gl.Viewport(window.Size);
        //Bind a texture and and set the uTexture0 to use texture0.

        Video.Texture.Bind(TextureUnit.Texture0);
        Shader.SetUniform("uTexture0", 0);

        Texture.Bind(TextureUnit.Texture1);
        Shader.SetUniform("uTexture1", 1);


        Shader.SetUniform("Saturation", Settings.Saturation / 100);
        Shader.SetUniform("KeyColor", Settings.KeyColor * 360);
        Shader.SetUniform("Brightness", Settings.Brightness / 100);
        Shader.SetUniform("Hue", Settings.Hue);

        Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);

        if (!VideoStop)
        {
            //for (var x = 0; x < 3; x++)
            //{
            //    Console.Write("{");
            //    for (var y = 0; y < 3; y++)
            //    {
            //        Console.Write(Video.KMeans[x, y] + " ");
            //    }

            //    Console.Write("}, ");
            //}
            //Console.WriteLine();

            Video.NextFrame();
            //Video.BindAndApplyShader();
            //Console.WriteLine(Video.FramePosition);
            //Console.WriteLine("BG image ID " + Video.GetBGTextureId(BlueH));

        }
        if (Video.FramePosition == 0)
        {
            var fps = 396 / (decimal)(DateTime.Now - DateNow).Seconds;
            Console.WriteLine($"{fps} fps");
            DateNow = DateTime.Now;
        }
        FormSetting.Refresh();

        //Console.WriteLine("render {0}", (time - DateTime.Now).TotalMilliseconds);
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
        FormSetting?.Dispose();
    }

    private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
    {
        if (arg2 == Key.Escape)
        {
            window.Close();
        }
        if (arg2 == Key.S)
        {
            VideoStop = !VideoStop;
        }
        if (arg2 == Key.N)
        {
            Video.NextFrame();
        }
    }
}
