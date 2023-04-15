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
using System.IO;

namespace PPBvCS
{
    class Program
    {
        private static IWindow window;
        private static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        private static ITexture Texture;
        private static ITexture Texture1;
        private static ITexture Texture2;
        private static ITexture Texture3;
        private static ITexture Texture4;
        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Silk.NET.Windowing.Window.Create(options);
            window.Load += OnLoad;
            window.Closing += OnClose;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            Gl = GL.GetApi(window);
            Texture = new BaseTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba8);
            Texture1 = new BaseTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba16);
            Texture2 = new BaseTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba16f);
            Texture3 = new BaseTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba32f);
            Texture4 = new BaseTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba4);

            using var image = Image.Load<Rgba32>(ResourcesProvider.Big);

            var width = image.Width;
            var height = image.Height;
            Vector3D<float> sum = new();
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var col = image[w, h];
                    sum += new Vector3D<float>() { X = col.R, Y = col.G, Z = col.B };
                }
            }
            var avarage = sum / (width * height);

            Console.WriteLine("avarage         " + avarage);
            Console.WriteLine("avarage         " + avarage / 255);
            Console.WriteLine("texture avarage " + Texture.AvgColor);
            Console.WriteLine("texture avarage " + Texture1.AvgColor);
            Console.WriteLine("texture avarage " + Texture2.AvgColor);
            Console.WriteLine("texture avarage " + Texture3.AvgColor);
            Console.WriteLine("texture avarage " + Texture4.AvgColor);

            Console.ReadLine();
            window.Close();
        }

        private static void OnClose()
        {
            //Remember to dispose all the instances.
            Gl.Dispose();
            Texture.Dispose();
            //RenderTarget.Dispose();
        }
    }
}