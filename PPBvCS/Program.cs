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
using System.IO;

namespace PPBvCS
{
    class Program
    {
        private static IWindow window;
        private static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        private static SharedResProject.Texture Texture;
        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Silk.NET.Windowing.Window.Create(options);
            window.Load += OnLoad;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            Gl = GL.GetApi(window);
            Texture = new (Gl, ResourcesProvider.Big);

            var image = Image.Load<Rgba32>(ResourcesProvider.Big);

            var width = image.Width;
            var height = image.Height;
            Vector3D<float> sum = new();
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var col = image[w, h];
                    sum += new Vector3D<float>() { X = col.R,Y = col.G,Z = col.B };
                }
            }
            var avarage = sum / (width * height);

            Console.WriteLine("avarage         " + avarage);
            Console.WriteLine("avarage         " + avarage/255);
            Console.WriteLine("texture avarage " + Texture.avgColor);

            Console.ReadLine();
            image.Dispose();

            Texture.Dispose();
            window.Dispose();
        }
    }
}